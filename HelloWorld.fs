// HelloWorld.fs
module HelloWorld

open System
open System.Net.Http
open System.Threading.Tasks

// Function to fetch web page content
let fetchWebPageContent (url: string) : Task<string> =
    task {
        use httpClient = new HttpClient()
        let! response = httpClient.GetAsync(url)

        // Check if the request was successful
        if response.IsSuccessStatusCode then
            let! content = response.Content.ReadAsStringAsync()
            return content
        else
            // Return an error message with the status code
            return $"Error: Failed to fetch content. Status code: {response.StatusCode} ({int response.StatusCode})"
    }

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
    0 // return an integer exit code