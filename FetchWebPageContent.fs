// FetchWebPageContent.fs
module FetchWebPageContent

open System.Net.Http
open System.Threading.Tasks

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