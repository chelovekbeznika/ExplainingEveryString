namespace ExplainingEveryString.Music.Model
{
    internal interface INote
    {
        Note Note { get; set; }
        Accidental Accidental { get; set; }
        NoteLength Length { get; set; }
    }
}
