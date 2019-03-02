using CryptoStringAPI.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CryptoStringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoStringController : ControllerBase
    {
        public CryptoStringController()
        {

        }

        [HttpPost, Route("Encrypt")]
        public async Task<EncryptResult> Encrypt([FromBody] EncryptCommand command)
        {
            EncryptResult result = new EncryptHandler().Handle(command);

            return await Task.FromResult(result);
        }

        [HttpPost, Route("Decrypt")]
        public async Task<DecryptResult> Decrypt([FromBody] DecryptCommand command)
        {
            DecryptResult result = new DecryptHandler().Handle(command);

            return await Task.FromResult(result);
        }
    }
}