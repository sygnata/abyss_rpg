using AbyssRpg.Api.Contracts.Characters;
using AbyssRpg.Application.Characters.Create;
using AbyssRpg.Application.Characters.GetById;
using Microsoft.AspNetCore.Mvc;

namespace AbyssRpg.Api.Controllers;

[ApiController]
[Route("api/characters")]
public sealed class CharactersController : ControllerBase
{
	private readonly CreateCharacterHandler _createCharacterHandler;
	private readonly GetCharacterByIdHandler _getCharacterByIdHandler;

	public CharactersController(
		CreateCharacterHandler createCharacterHandler,
		GetCharacterByIdHandler getCharacterByIdHandler)
	{
		_createCharacterHandler = createCharacterHandler;
		_getCharacterByIdHandler = getCharacterByIdHandler;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateCharacterRequest request, CancellationToken cancellationToken)
	{
		CreateCharacterCommand command = new(request.Name);

		CreateCharacterResult result = await _createCharacterHandler.HandleAsync( command, cancellationToken );

		return CreatedAtAction( 
			nameof(GetById), 
			new { id = result.Id }, 
			result 
		);
	}

	[HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById( Guid id, CancellationToken cancellationToken)
	{
		GetCharacterByIdQuery query = new(id);

		GetCharacterByIdResult result = await _getCharacterByIdHandler.HandleAsync( query, cancellationToken );

		return Ok(result);
	}
}