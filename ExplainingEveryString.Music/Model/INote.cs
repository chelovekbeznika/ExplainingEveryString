using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Music.Model
{
    internal interface INote
    {
        Note Note { get; set; }
        Accidental Accidental { get; set; }
        NoteLength Length { get; set; }
    }
}
