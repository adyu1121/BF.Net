using System.IO;

namespace BFNet
{
    interface IO
    {
        public char Input();
        public void Output(char ASCII);
    }
    public class StreamIO : IO
    {
        TextWriter writer;
        TextReader reader;

        public char Input()
        {
            return (char)reader.Read();
        }

        public void Output(char ASCII)
        {
            writer.Write(ASCII);
        }
        public StreamIO(TextReader reader, TextWriter writer)
        {
            SetIO(reader, writer);
        }
        public void SetIn(TextReader reader)
        {
            this.reader = reader;
        }
        public void SetOut(TextWriter writer)
        {
            this.writer = writer;
        }
        public void SetIO(TextReader reader, TextWriter writer)
        {
            SetIn(reader);SetOut(writer);
        }
    }
}
