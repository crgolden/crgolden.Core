namespace Clarity.Core
{
    using MediatR;

    public abstract class RemoveRequest<TKey> : IRequest
    {
        public readonly string[] FileNames;

        public readonly TKey[][] Keys;

        protected RemoveRequest(string[] fileNames, TKey[][] keys = null)
        {
            FileNames = fileNames;
            Keys = keys;
        }
    }
}
