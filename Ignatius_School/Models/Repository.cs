namespace Ignatius_School.Models
{
    public class Repository:IRepository
    {
        private Dictionary<string, Events> ListOfEvents;
        public Repository() 
        {
        ListOfEvents = new Dictionary<string, Events>();

            ListOfEvents.Add("1",new Events {Name = "First event", Description = "This is my first event",DateOfCreate = DateTime.Now });
            ListOfEvents.Add("2",new Events {Name = "Second event", Description = "This is my Second event", DateOfCreate = DateTime.Now });
            ListOfEvents.Add("3",new Events {Name = "Third event", Description = "This is my Third event", DateOfCreate = DateTime.Now });
            ListOfEvents.Add("4",new Events {Name = "fourth event", Description = "This is my fourth event", DateOfCreate = DateTime.Now });
        
        }
        public List<Events> Events()
        {
            return ListOfEvents.Values.ToList();

        }

    }
}
