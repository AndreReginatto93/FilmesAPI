﻿using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class CreateFilmeDto
{
    public string titulo { get; set; }

    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    public string genero { get; set; }

    [Required(ErrorMessage = "A duração do filme é obrigatório")]
    [Range(60, 300, ErrorMessage = "A duração deve ter entre 60 e 300 minutos")]
    public int duracao { get; set; }
}
