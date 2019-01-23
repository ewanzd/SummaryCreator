using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomaticSummaryCreator.Data
{
    /// <summary>
    /// Load and save all data.
    /// </summary>
    public class Summary : IEnumerable<TableContainer>
    {
        /// <summary>
        /// Container for data.
        /// </summary>
        List<TableContainer> container = new List<TableContainer>();

        /// <summary>
        /// The first value in all files.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                // Gibt der erste Wert der Liste zurück
                return container.Min(x => x.FirstTime);
            }
        }

        /// <summary>
        /// The last value in all files.
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                // Gibt der letzte Wert der Liste zurück
                return container.Max(x => x.LastTime);
            }
        }

        /// <summary>
        /// Return count of containers.
        /// </summary>
        public int Count
        {
            get
            {
                // Gibt die Anzahl Elemente der Liste zurück
                return container.Count;
            }
        }

        /// <summary>
        /// Add a new table to the table with the same ID. When don't found the table, the method create a new one.
        /// </summary>
        /// <param name="id">Add the row to the table with this ID.</param>
        /// <param name="row">The new row.</param>
        public virtual void Add(string id, Row row)
        {
            // Abfrage für die Liste. Muss eine Tabelle sein
            // und dieselbe ID besitzen
            var temp = from n in container
                       where n is Table
                       where n.ID == id
                       select n;

            // Tabelle, um später die neue Zeile hinzuzufügen
            Table table;

            // Prüft, ob es eine Tabelle mit der übergeben ID gibt
            if(temp.Count() < 1)
            {
                // Falls nein, wird hier nun eine neue Tabelle erstellt
                table = new Table();
                table.ID = id;
                container.Add(table);
            }
            else
            {
                // Falls ja wird die erste Tabelle mit der übergegebene ID abgerufen
                table = (Table)temp.First();
            }

            // Zeile der Tabelle hinzufügen
            table.Add(row);
        }

        /// <summary>
        /// Add new container to the list.
        /// </summary>
        /// <param name="newContainer">The new container for the summary.</param>
        public virtual void Add(TableContainer newContainer)
        {
            // Der neue Container darf nicht null sein, da es sonst zu Fehler führen kann
            if(newContainer == null)
                throw new ArgumentNullException("newContainer");

            // Fügt der neue Container der Liste hinzu
            container.Add(newContainer);
        }

        /// <summary>
        /// Sort all containers with sequence id's.
        /// </summary>
        /// <param name="sequence">So should be the sequence of containers.</param>
        public void SortContainerSequence(List<string> ids)
        {
            // Neue Liste
            List<TableContainer> newList = new List<TableContainer>();

            // Jede ID in der richtigen Reihenfolge
            foreach(var id in ids)
            {
                // Item mit der ID suchen
                TableContainer toMove = container.Find(x => x.ID == id);

                // Falls gefunden einfügen
                if(toMove != null)
                {
                    container.Remove(toMove);
                    newList.Add(toMove);
                }
            }

            // Die übrigen Items werden hinten angefügt
            foreach(var rest in container)
                newList.Add(rest);

            // Neue Liste speichern
            container = newList;
        }

        /// <summary>
        /// Sort all containers.
        /// </summary>
        public void Sort()
        {
            // Sortiert alle Container der Liste
            foreach(var item in container)
                item.Sort();
        }

        /// <summary>
        /// Move a item one step to top.
        /// </summary>
        /// <param name="item">The item, that should move.</param>
        /// <returns>Return true when successful else return false.</returns>
        public bool MoveTop(TableContainer item)
        {
            // Item suchen
            int idx = container.FindIndex(x => x == item);
            if(idx > 0)
            {
                // Falls gefunden wird es um eine Position nach oben verschoben
                container.Remove(item);
                container.Insert(idx - 1, item);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Move a item one step to down.
        /// </summary>
        /// <param name="item">The item, that should move.</param>
        /// <returns>Return true when successful else return false.</returns>
        public bool MoveDown(TableContainer item)
        {
            // Item suchen
            int idx = container.FindIndex(x => x == item);
            if(idx < container.Count - 1)
            {
                // Falls gefunden wird es um eine Position nach unten verschoben
                container.Remove(item);
                container.Insert(idx + 1, item);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Search a TableContainer with Predicate.
        /// </summary>
        /// <param name="condition">Condition of search.</param>
        /// <returns>Return the result of condition.</returns>
        public TableContainer Find(Predicate<TableContainer> condition)
        {
            // Gibt die Suchauswertung zurück.
            return container.Find(condition);
        }

        /// <summary>
        /// Remove all items.
        /// </summary>
        public virtual void Clear()
        {
            // Löscht alle Elemente
            container.Clear();
        }

        /// <summary>
        /// Remove all items with that id.
        /// </summary>
        /// <param name="id">The id of item.</param>
        /// <returns>Successfull or not.</returns>
        public virtual bool Remove(TableContainer obj)
        {
            // löscht alle Container mit dieser ID
            return container.Remove(obj);
        }

        /// <summary>
        /// Get all container.
        /// </summary>
        /// <returns>Return every container.</returns>
        public virtual IEnumerator<TableContainer> GetEnumerator()
        {
            // Gibt jedes Element der Container-Liste zurück
            foreach(var item in container)
                yield return item;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
