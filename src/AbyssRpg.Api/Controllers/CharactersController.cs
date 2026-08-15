using AbyssRpg.Api.Contracts.Characters;
using AbyssRpg.Application.Characters.Create;
using Microsoft.AspNetCore.Mvc;

namespace AbyssRpg.Api.Controllers;

[ApiController]
[Route("api/characters")]
public sealed class CharactersController : ControllerBase
{
	private readonly CreateCharacterHandler
		_createCharacterHandler;

	public CharactersController(
		CreateCharacterHandler createCharacterHandler)
	{
		_createCharacterHandler =
			createCharacterHandler;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateCharacterRequest request, CancellationToken cancellationToken)
	{
		CreateCharacterCommand command = new(request.Name);

		CreateCharacterResult result =
			await _createCharacterHandler.HandleAsync(
				command,
				cancellationToken
			);

		return Created(
			$"/api/characters/{result.Id}",
			result
		);
	}
}