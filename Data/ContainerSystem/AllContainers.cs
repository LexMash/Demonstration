using System.Collections.Generic;

namespace FromTheBasement.Data.ContainerSystem
{
    public class AllContainers
    {
        public Dictionary<string, Container> ContainersMap = new Dictionary<string, Container>();

        public AllContainers(Container[] containers)
        {
            foreach(Container container in containers)
            {
                ContainersMap.Add(container.Id, container);
            }
        }
    }
}
