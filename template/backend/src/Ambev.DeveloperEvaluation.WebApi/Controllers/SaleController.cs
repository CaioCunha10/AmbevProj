using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ILogger<SaleController> _logger;


        public SaleController(ISaleService saleService, ILogger<SaleController> logger)
        {
            _saleService = saleService;
            _logger = logger;
        }

        #region GET: api/sale
        /// <summary>
        /// Retorna todas as vendas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SaleReadDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? cliente, [FromQuery] bool? cancelado, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Iniciando a consulta de todas as vendas com filtro de cliente: {Cliente}, cancelado: {Cancelado}, página: {Page}, pageSize: {PageSize}", cliente, cancelado, page, pageSize);

            var result = await _saleService.GetFilteredAsync(cliente, cancelado, page, pageSize);
            _logger.LogInformation("Consulta realizada com sucesso. Total de resultados encontrados: {TotalResultados}", result.Count());

            return Ok(result);
        }

        #endregion

        #region GET: api/sale/{id}
        /// <summary>
        /// Retorna uma venda específica por ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaleReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Consultando venda com ID: {SaleId}", id);

            var result = await _saleService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
        #endregion

        #region POST: api/sale
        /// <summary>
        /// Cria uma nova venda.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SaleReadDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] SalePostDTO dto)
        {
            _logger.LogInformation("Criando nova venda com os dados: {@SalePostDTO}", dto);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo inválido ao tentar criar venda. Erros de validação: {ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _saleService.PostAsync(dto);
                _logger.LogInformation("Venda criada com sucesso com ID: {SaleId}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro ao tentar criar venda");
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region PUT: api/sale/{id}
        /// <summary>
        /// Atualiza uma venda existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaleReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(Guid id, [FromBody] SalePutDTO dto)
        {
            try
            {
                var result = await _saleService.PutAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        #endregion

        #region PATCH: api/sale/{id}/cancel
        /// <summary>
        /// Cancela uma venda.
        /// </summary>
        [HttpPatch("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var success = await _saleService.CancelAsync(id);
            return success ? Ok() : NotFound();
        }
        #endregion

        #region PATCH: api/sale/{saleId}/item/{itemId}/cancel
        /// <summary>
        /// Cancela um item específico de uma venda.
        /// </summary>
        [HttpPatch("{saleId}/item/{itemId}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelItem(Guid saleId, Guid itemId, [FromBody] SaleItemCancelDTO dto)
        {
            try
            {
                var result = await _saleService.CancelItemAsync(saleId, itemId, dto);
                return result ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion
    }
}
