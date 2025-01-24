using API.RESTful.DTO;
using Core.Domain;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.RESTful.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PakketController : ControllerBase
    {
        private readonly IPakketRepo _pakketRepository;
        private readonly IStudentRepo _studentRepository;
        private readonly IKantineRepo _kantineRepository;
        private readonly IPakketService _pakketService;

        public PakketController(IPakketRepo pakketRepository, IStudentRepo studentRepo, IKantineRepo kantineRepo, IPakketService pakketService)
        {
            _pakketRepository = pakketRepository;
            _studentRepository = studentRepo;
            _kantineRepository = kantineRepo;
            _pakketService = pakketService;
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(typeof(IEnumerable<Pakket>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetPakketten()
        {
            var pakketten = _pakketRepository.GetPakketten();
            return Ok(pakketten);
        }

        [HttpGet("gereserveerd")]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(typeof(IEnumerable<Pakket>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetGereserveerd()
        {
            try
            {
                var gereserveerdePakketten = _pakketRepository.GetAlleGereserveerdePakketten();
                return Ok(gereserveerdePakketten);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reserveer")]
        [ServiceFilter(typeof(AuthFilter))]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public IActionResult ReserveerPakket([FromBody] ReserveerPakketReq req)
        {
            try
            {
                if (req == null)
                {
                    return BadRequest(new { error = "Onjuist object" });
                }

                var studentBday = _studentRepository.GetStudentById(req.studentId).geboortedatum;
                var pakket = _pakketRepository.GetPakketById(req.pakketId);
                var reserveerDatum = pakket.pickup.Date;
                var studentLeeftijd = DateTime.Now.Year - studentBday.Year;
                var pakketLeeftijd = pakket.volwassen;
                var kantines = _kantineRepository.GetKantines();

                var bestaandeReservatieDatum = _pakketRepository.GetGereserveerdePakketten(req.studentId)
                    .Where(p => p.pickup.Date == reserveerDatum)
                    .ToList();

                if (pakketLeeftijd && studentLeeftijd<18)
                {
                    return BadRequest(new { error = "Je moet minimaal 18 jaar zijn om dit pakket te kunnen reserveren!" });
                }

                if (bestaandeReservatieDatum.Any())
                {
                    return BadRequest(new { error = "Je hebt al een pakket gereserveerd op deze datum!" });
                }

                if(pakket.gereserveerd != null)
                {
                    return BadRequest(new { error = "Dit pakket is al gereserveerd!" });
                }

                if(!_pakketService.reserveerPakket(req.pakketId, req.studentId))
                {
                    return BadRequest(new { error = "Dit pakket is niet beschikbaar!" });
                }

                return Ok(new { message = "Pakket succesvol gereserveerd!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}