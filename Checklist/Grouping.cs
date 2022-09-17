using System;
using System.Collections.Generic;
using System.Text;

namespace Checklistion.Checklist
{
    // TODO: Docstring
    public class Grouping
    {
        // TODO: Docstring
        public class Subgroup
        {
            public readonly Group parentGroup;

            public readonly string name;

            public List<Entry> entries = new List<Entry>();

            public Subgroup(Group parentGroup, string name)
            { 
                this.parentGroup = parentGroup;
                this.name = name;
            }

            public void AddEntry(System.IO.FileInfo file, uint fileLine, string id, string requirement)
            {
                this.entries.Add(
                    new Entry(
                        file, 
                        fileLine, 
                        this.parentGroup.name, 
                        this.name, 
                        id,
                        requirement));
            }
        }

        // TODO: Docstring
        public class Group
        { 
            public readonly string name;

            public Dictionary<string, Subgroup> subGroups = new Dictionary<string, Subgroup>();

            public Group(string groupName)
            { 
                this.name = groupName;
            }

            public Subgroup GetSub(string subName)
            { 
                if(this.subGroups.ContainsKey(subName))
                    return this.subGroups[subName];

                Subgroup newSub = new Subgroup(this, subName);
                this.subGroups.Add(subName, newSub);
                return newSub;
            }
        }

        public Dictionary<string, Group> groups = new Dictionary<string, Group>();

        // TODO: Docstring
        public Group GetGroup(string groupName)
        { 
            if(this.groups.ContainsKey(groupName))
                return this.groups[groupName];

            Group newGroup = new Group(groupName);
            this.groups.Add(groupName, newGroup);
            return newGroup;
        }
    }
}
