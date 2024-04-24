using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Domain.Abstractions;
public interface IImageService
{
    Task<string> StoreAsync(IFormFile image);
    void Delete(string filePath);
}
