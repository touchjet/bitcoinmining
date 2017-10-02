using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FusionMiner.Domain.DTO;

namespace FusionMiner
{
	public class HostApiClient
	{
		public static async Task<MinerMaintPackDTO> SubmitStatus (MinerStatDTO status)
		{
			return await ApiPost<MinerMaintPackDTO,MinerStatDTO> ("api/Miners/Status", status);
		}

		public static async Task<MinerConfigDTO> UpdateConfig (MinerConfigDTO config)
		{
			return await ApiPost<MinerConfigDTO,MinerConfigDTO> ("api/Miners/Config", config);
		}

		private static async Task<T> ApiGet<T> (string api)
		{
			try {
				using (var client = new HttpClient ()) {
					client.Timeout = new TimeSpan (0, 0, 20);
					client.BaseAddress = new Uri (Config.HostApiUrl);
					client.DefaultRequestHeaders.Accept.Clear ();
					client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
					HttpResponseMessage response = await client.GetAsync (api);
					if (response.IsSuccessStatusCode) {
						return await response.Content.ReadAsAsync<T> ();
					}
				}
			} catch {
			}
			return default(T);
		}

		private static async Task<T> ApiPost<T,K> (string api, K request)
		{
			try {
				using (var client = new HttpClient ()) {
					client.Timeout = new TimeSpan (0, 0, 20);
					client.BaseAddress = new Uri (Config.HostApiUrl);
					client.DefaultRequestHeaders.Accept.Clear ();
					client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
					HttpResponseMessage response = await client.PostAsJsonAsync<K> (api, request);
					if (response.IsSuccessStatusCode) {
						return await response.Content.ReadAsAsync<T> ();
					}
				}
			} catch {
			}
			return default(T);
		}
	}
}

