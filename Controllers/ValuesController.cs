using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Google.Cloud.Speech.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoogleSpeechToTextRestService.Controllers
{
    [Route("api")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        static string credential_path = @"C:\6f23398153bf.json";
        [HttpPost]
        [Route("speech2text")]
        public ActionResult<IEnumerable<string>> Post(IFormFile content)
        {
            
            List<string> returnResult = new List<string>();
            if (content == null)
            {
                throw new Exception("Dosya Bulunamadı!");
            }

            try
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
                var speech = SpeechClient.Create();
                if (content.Length > 0)
                {
                    var response = speech.Recognize(new RecognitionConfig()
                    {
                        Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                        SampleRateHertz = 8000,
                        LanguageCode = "tr-TR",
                    }, RecognitionAudio.FromStream(content.OpenReadStream()));

                    foreach (var result in response.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            returnResult.Add(alternative.Transcript);
                        }
                    }
                }

                return returnResult;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
    }
}
