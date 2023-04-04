namespace App.Services
{
    public class SummerNote
    {
        public SummerNote(string idEditor, bool loadSummer = true)
        {
            IdEditor = idEditor;
            LoadSummer =loadSummer;
        }
        public string IdEditor {set;get;}

        public bool LoadSummer{set;get;}

        public  int height {set;get;} = 120;

        public string toolbar {set;get;} = @"
        [
          ['style', ['style']],
          ['font', ['bold', 'underline', 'clear']],
          ['color', ['color']],
          ['para', ['ul', 'ol', 'paragraph']],
          ['table', ['table']],
          ['insert', ['link', 'picture', 'video','elfinder']],
          ['height', ['height']],
          ['view', ['fullscreen', 'codeview', 'help']]
        ]
        ";
    }
    
}