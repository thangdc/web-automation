
namespace ThangDC.Core.Utilities
{
    /// <summary>
    /// represents a word
    /// </summary>
	public class Word
	{
        public Word() { }
        public string Text { get; set; }
        public string AttachedText { get; set; }
        public bool IsShellCommand { get; set; }
	}
}
