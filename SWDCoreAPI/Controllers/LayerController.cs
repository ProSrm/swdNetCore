using Microsoft.AspNetCore.Mvc;
using SWDCoreAPI.Models;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;


[Route("api/[controller]")]
[ApiController]
public class LayerController : ControllerBase
{
    private readonly ILayerService _layerService;

    public LayerController(ILayerService layerService)
    {
        _layerService = layerService;
    }

    [HttpGet("getLayerType/{layerName}")]
    public async Task<IActionResult> GetLayerType(string layerName)
    {
        var result = await _layerService.GetLayerType(layerName);
        return result != null ? Ok(result) : NotFound("Layer not found.");
    }


    // [HttpPost("addupdatelayerformdata")]
    // public async Task<IActionResult> AddUpdateLayerFormData([FromForm] LayerFormData model)
    //     {
    //         try
    //         {
    //             if (model == null || string.IsNullOrEmpty(model.LayerName))
    //                 return BadRequest("Invalid request: data is missing.");

    //             string uploadFolderPath = Path.Combine("wwwroot", "uploads", model.LayerName);

    //             // Ensure directory exists
    //             if (!Directory.Exists(uploadFolderPath))
    //                 Directory.CreateDirectory(uploadFolderPath);

    //             string filePath = null;

    //             if (model.File != null)
    //             {
    //                 string uniqueFileName = $"{Path.GetFileNameWithoutExtension(model.File.FileName)}-{DateTime.Now.Ticks}{Path.GetExtension(model.File.FileName)}";
    //                 filePath = Path.Combine(uploadFolderPath, uniqueFileName);

    //                 using (var stream = new FileStream(filePath, FileMode.Create))
    //                 {
    //                     await model.File.CopyToAsync(stream);
    //                 }

    //                 filePath = $"/uploads/{model.LayerName}/{uniqueFileName}";
    //             }

    //             using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
    //             {
    //                 await conn.OpenAsync();

    //                 string layerTypeQuery = "EXEC spLayerMaster_GetLayerGeomType @LayerName";
    //                 using (SqlCommand cmd = new SqlCommand(layerTypeQuery, conn))
    //                 {
    //                     cmd.Parameters.AddWithValue("@LayerName", model.LayerName);
    //                     string layerType = (string)await cmd.ExecuteScalarAsync();

    //                     string geom = layerType == "POINT"
    //                         ? $"POINT({model.Longitude} {model.Latitude})"
    //                         : $"MULTILINESTRING(({model.Longitude} {model.Latitude}))";

    //                     string sqlQuery = model.QgsFid.HasValue
    //                         ? "UPDATE Layers SET Geom = @Geom, ImagePath = @ImagePath, ModifiedBy = 1 WHERE QgsFid = @QgsFid"
    //                         : "INSERT INTO Layers (LayerName, Geom, ImagePath, CreatedBy) VALUES (@LayerName, @Geom, @ImagePath, 1)";

    //                     using (SqlCommand updateCmd = new SqlCommand(sqlQuery, conn))
    //                     {
    //                         updateCmd.Parameters.AddWithValue("@LayerName", model.LayerName);
    //                         updateCmd.Parameters.AddWithValue("@Geom", geom);
    //                         updateCmd.Parameters.AddWithValue("@ImagePath", filePath ?? (object)DBNull.Value);
    //                         if (model.QgsFid.HasValue)
    //                             updateCmd.Parameters.AddWithValue("@QgsFid", model.QgsFid);

    //                         await updateCmd.ExecuteNonQueryAsync();
    //                     }
    //                 }
    //             }

    //             return Ok(new { message = model.QgsFid.HasValue ? "Data updated successfully." : "Data added successfully." });
    //         }
    //         catch (Exception ex)
    //         {
    //             return StatusCode(500, new { error = ex.Message });
    //         }
    //     }
    // }


    // [HttpPost("addUpdateLayerFormData")]
    // [Consumes("multipart/form-data")] 
    // public async Task<IActionResult> AddUpdateLayerFormData([FromForm] LayerFormData data)
    // {

    //     if (data.ImageFile != null)
    //     {
    //         var uploadsPath = Path.Combine("uploads","LoadingImages");
    //         Directory.CreateDirectory(uploadsPath);
    //         var filePath = Path.Combine(uploadsPath, file.FileName);
    //         using (var stream = new FileStream(filePath, FileMode.Create))
    //         {
    //             await file.CopyToAsync(stream);
    //         }
    //         data.ImagePath = filePath;
    //     }
    //     var result = await _layerService.AddOrUpdateLayer(data);
    //     return Ok(new { message = "Data added/updated successfully.", affectedRows = result });
    // }

    [HttpPost("addUpdateLayerFormData")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddUpdateLayerFormData([FromForm] LayerFormData data)
    {
        if (data.ImageFile != null)
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "LoadingImages");
            Directory.CreateDirectory(uploadsPath);

            var filePath = Path.Combine(uploadsPath, data.ImageFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await data.ImageFile.CopyToAsync(stream);
            }

            data.ImagePath = filePath; // Store the path in the database
        }

        var result = await _layerService.AddOrUpdateLayer(data);
        return Ok(new { message = "Data added/updated successfully.", affectedRows = result });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginToPortal([FromBody] Login data)
    {
        if (data.UserName == null || data.Password == null)
        {
            return BadRequest(new { message = "Username and Password are required." });
        }


        var result = await _layerService.LoginToPortal(data);

        if (result == 1)
        {
            return Ok(new { message = "User logged in successfully." });
        }


        else
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupModel data)
    {
        if (data == null)
        {
            return BadRequest(new { message = "Invalid input." });
        }

        if (data.Password != data.ConfirmPassword)
        {
            return BadRequest(new { message = "Passwords do not match." });
        }

        var result = await _layerService.SignupUser(data);

        if (result == -1)
        {
            return Conflict(new { message = "Email already exists." });
        }

        return Ok(new { message = "User registered successfully." });
    }


    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _layerService.GetAllUsers();
        return Ok(users);
    }


    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UserModel data)
    {
        if (data == null)
        {
            return BadRequest(new { message = "Invalid input." });
        }

        var result = await _layerService.UpdateUser(data);

        if (result == -1)
        {
            return NotFound(new { message = "User not found." });
        }

        if (result == -2)
        {
            return Conflict(new { message = "Email already exists for another user." });
        }

        return Ok(new { message = "User updated successfully." });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Invalid user ID." });
        }

        var result = await _layerService.DeleteUser(id);

        if (result == -1)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(new { message = "User deleted successfully." });
    }
}
