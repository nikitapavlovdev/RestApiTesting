using System.Text;
using Newtonsoft.Json;
using EfMobWebApiApp.Models;

namespace EfMobApiTest
{
    public class TTaskTest
    {
        [Fact]
        public async Task UploadFile_Test()
        {
            HttpClient client = new()
            {
                BaseAddress = new("http://localhost:5207/")
            };

            string fileContent = """
            яндекс.ƒирект:/ru
            –евдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
            √азета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
             рута€ реклама:/ru/svrd
            """;

            using StringContent content = new(
                fileContent,
                Encoding.UTF8,
                "text/plain"
            );

            HttpResponseMessage streamResponse = await client.PostAsync("efmob/upload", content);

            UploadResponse? uploadResponse = JsonConvert.DeserializeObject<UploadResponse>(await streamResponse.Content.ReadAsStringAsync());

            Assert.NotNull(uploadResponse);
            Assert.True(uploadResponse.Success);
        }

        [Fact]
        public async Task SearchPlatform_Test()
        {
            HttpClient client = new()
            {
                BaseAddress = new("http://localhost:5207/")
            };

            HttpResponseMessage streamResponse = await client.GetAsync("efmob/search?route=/de");

            SearchResponse<List<string>>? searchResponse = JsonConvert.DeserializeObject<SearchResponse<List<string>>>(await streamResponse.Content.ReadAsStringAsync());

            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.Success);
            Assert.Equal("Meta Ads", searchResponse?.Data?[1]);
        }
    }
}