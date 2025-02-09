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

    let urlsAndHtml = extractUrls(content)

    // Print the URLs and inner HTML
    urlsAndHtml |> List.iter (fun (url, innerHtml) -> printfn "URL: %s\nInner HTML: %s\n" url innerHtml)

    0 // return an integer exit code