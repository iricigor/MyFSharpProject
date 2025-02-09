module MyWebFunctions

open System.Net.Http
open System.Text
open System.Threading.Tasks
open System.IO
open HtmlAgilityPack

let fetchWebPageContent (url: string) : Task<string> =
    task {
        use httpClient = new HttpClient()
        let! response = httpClient.GetAsync(url)

        // Check if the request was successful
        if response.IsSuccessStatusCode then
            printfn "The page read successfully"
            use stream = response.Content.ReadAsStream()
            use reader = new StreamReader(stream, Encoding.UTF8) // Ensure UTF-8 encoding is used
            let! content = reader.ReadToEndAsync()
            return content
        else
            // Return an error message with the status code
            return $"Error: Failed to fetch content. Status code: {response.StatusCode} ({int response.StatusCode})"
    }

let extractUrls (html: string) : (string * string) list =
    // Load HTML into HtmlDocument
    let doc = new HtmlDocument()
    doc.LoadHtml(html)

    // Select all <div class="art"> elements using XPath
    let nodes = doc.DocumentNode.SelectNodes("//div[@class='art  ']")

    // Extract the href attribute and inner HTML for each <a> element inside the selected nodes
    [
        for i in 0 .. nodes.Count - 1 do
            let node = nodes.[i]
            let aTag = node.SelectSingleNode(".//a[@href]")
            if aTag <> null then
                yield (aTag.GetAttributeValue("href", ""), aTag.InnerHtml)
    ]
