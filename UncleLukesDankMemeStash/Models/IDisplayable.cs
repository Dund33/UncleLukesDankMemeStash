namespace UncleLukesDankMemeStash.Models
{
    public interface IDisplayable
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
    }
}