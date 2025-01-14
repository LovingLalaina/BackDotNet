using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs.Files;
using back_dotnet.Services.Files;
using back_dotnet.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace back_dotnet.Controllers
{
  [ApiController]
  [Route("file")]
  public class FileController : ControllerBase
  {
    private readonly IFileService fileService;

    public FileController(IFileService fileService)
    {
        this.fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      try
      {
        var files = await fileService.GetAllAsync();
        if(files is null) return StatusCode(StatusCodes.Status404NotFound, new {status = StatusCodes.Status404NotFound, error = "Aucun fichier trouvé"});
        return Ok(files);
      }
      catch (System.Exception e)
      {
        var error = e as HttpException;
        if( error is not null) {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message});
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error"});
      }
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
      try
      {
        var file = await fileService.GetByIdAsync(id);
        if(file is null) return StatusCode(StatusCodes.Status404NotFound, new {status = StatusCodes.Status404NotFound, error = "Aucun fichier trouvé"});
        return Ok(file);
      }
      catch (System.Exception e)
      {
        var error = e as HttpException;
        if( error is not null) {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message});
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error"});
      }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] UploadImageDto uploadImage)
    {
      try
      {
        var file = await fileService.CreateAsync(uploadImage);
        if(file is null) return StatusCode(StatusCodes.Status500InternalServerError, new {status = StatusCodes.Status500InternalServerError, error = "Fichier non enregistré"});
        return StatusCode(StatusCodes.Status201Created, file);
      }
      catch (Exception e)
      {
        var error = e as HttpException;
        if( error is not null) {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message});
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error"});
      }
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
      try
      {
        var file = await fileService.DeleteAsync(id);
        if(file != null) return Ok(id);
        return StatusCode(StatusCodes.Status404NotFound, new { status = StatusCodes.Status404NotFound, error = "Fichier introuvable"});
      }
      catch (Exception e)
      {
        var error = e as HttpException;
        if( error is not null) {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message});
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error"});
      }
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateAsync([FromForm] UploadImageDto uploadImage, [FromRoute] Guid id)
    {
      try
      {
        var file = await fileService.UpdateAsync(id, uploadImage);
        if(file is null) return StatusCode(StatusCodes.Status404NotFound, new {status = StatusCodes.Status404NotFound, error = "Aucun fichier trouvé"});
        return Ok(file);
      }
      catch (Exception e)
      {
        var error = e as HttpException;
        if( error is not null) {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message});
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error"});
      }
    }
  }
}