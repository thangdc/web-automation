using System;
namespace WebAutomation.DownloadManager
{
    [Serializable]
    public struct CalculatedSegment
    {
        private long startPosition;
        private long endPosition;

        public long StartPosition
        {
            get { return startPosition; }
        }

        public long EndPosition
        {
            get { return endPosition; }
        }

        public CalculatedSegment(long startPos, long endPos)
        {
            this.endPosition = endPos;
            this.startPosition = startPos;
        }
    }
}
