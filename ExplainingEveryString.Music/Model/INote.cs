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
        Alteration Alteration { get; set; }
        NoteLength Length { get; set; }
    }
}
