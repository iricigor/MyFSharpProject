// HelloWorld.fs
module HelloWorld

open System
open System.Net.Http
open System.Threading.Tasks
open MyWebFunctions
open HtmlAgilityPack

[<EntryPoint>]
let main argv =
    // Default URL
    let defaultUrl = "https://example.com"

    printf "Please enter a URL (or press Enter to use the default): "
    let userUrl = Console.ReadLine()

    // Use the default URL if the user doesn't provide one
    let url = if String.IsNullOrWhiteSpace(userUrl) then defaultUrl else userUrl

    // Fetch the web page content asynchronously
    let content = fetchWebPageContent url |> Async.AwaitTask |> Async.RunSynchronously

    // Print the length of the content
    printfn "\nLength of the web page content: %d characters" content.Length

    // Load HTML into HtmlDocument
    let doc = new HtmlDocument()
    doc.LoadHtml(content)

    // Select all <div class="art"> elements using XPath
    let nodes = doc.DocumentNode.SelectNodes("//div[@class='art  ']")

    // Print the index and the href attribute of each <a> element inside the selected nodes
    for i in 0 .. nodes.Count - 1 do
        let node = nodes.[i]
        let aTag = node.SelectSingleNode(".//a[@href]")
        if aTag <> null then
            printfn "%d: %s" (i + 1) (aTag.GetAttributeValue("href", ""))

    0 // return an integer exit code