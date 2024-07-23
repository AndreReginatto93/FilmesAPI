using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using FilmesAPI.Profiles;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Recupera vários filmes do banco de dados
    /// </summary>
    /// <param name="skip">Quantos filmes pular para fazer paginação</param>
    /// <param name="take">Quantos filmes mostrar por página</param>
    /// <returns>IEnumerable</returns>
    /// <response code="200">Caso recupere com sucesso</response>
    [HttpGet]
    public IEnumerable<ReadFilmeDto> getFilmes([FromQuery]int skip = 0,[FromQuery] int take = 10)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Recupera um filme do banco de dados
    /// </summary>
    /// <param name="id">Id do filme a ser recuperardo</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso recupere com sucesso</response>
    [HttpGet("{id}")]
    public IActionResult getFilmeById(int id)
    {
        var filme = _context.filmes.FirstOrDefault(filme => filme.id == id);

        if (filme == null) return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Adiciona um filme no banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    public IActionResult createFilme([FromBody] CreateFilmeDto filmeDto) {
        Filme filme = _mapper.Map<Filme>(filmeDto);

        _context.filmes.Add(filme);
        _context.SaveChanges();
        Console.WriteLine(filme.titulo);

        return CreatedAtAction(nameof(getFilmeById),new { id = filme.id }, filme);
    }


    /// <summary>
    /// Atualiza um filme no banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para atualização de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso atualização seja feita com sucesso</response>
    [HttpPut("{id}")]
    public IActionResult updateFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.filmes.FirstOrDefault(filme => filme.id == id);

        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Atualiza um campo do filme no banco de dados
    /// </summary>
    /// <param name="JsonPatchDocument">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso atualização seja feita com sucesso</response>
    [HttpPatch("{id}")]
    public IActionResult patchFilme(int id, [FromBody] JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.filmes.FirstOrDefault(filme => filme.id == id);

        if (filme == null) return NotFound();

        var filmeToUpdate = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeToUpdate, ModelState);

        if (!TryValidateModel(filmeToUpdate))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeToUpdate, filme);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Exclui um filme no banco de dados
    /// </summary>
    /// <param name="id">Id do filme a ser excluido</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso exclusão seja feita com sucesso</response>
    [HttpDelete("{id}")]
    public IActionResult deleteFilme(int id) {
        var filme = _context.filmes.FirstOrDefault(filme => filme.id == id);

        if (filme == null) return NotFound();

        _context.Remove(filme);
        _context.SaveChanges();

        return NoContent();
    }
}
