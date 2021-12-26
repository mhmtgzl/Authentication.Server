using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Shared.DTO;

public class ErrorDto
{
    public List<string> Errors { get; set; } = new List<string>();
    public bool IsShow { get; set; }=true;
    
    public ErrorDto(string error,bool isShow)
    {
        this.Errors.Add(error);
        this.IsShow = isShow;
    }

    public ErrorDto(List<string> errors,bool isShow)
    {
        this.Errors = errors;
        this.IsShow = isShow;
    }


}

