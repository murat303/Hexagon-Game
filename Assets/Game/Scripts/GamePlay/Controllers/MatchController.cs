using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexagon
{
    public class MatchController : Singleton<MatchController>
    {
        readonly Queue<Group> matches = new Queue<Group>();

        public bool MatchFound { get; set; }

        public IEnumerator CheckMatches()
        {
            MatchFound = FindAllMatches();

            if (MatchFound)
            {
                HandleAllMatches();
                Highlighter.Instance.Disable();
                yield return new WaitForSeconds(.25f);
                yield return ShiftAndFill();
                yield return new WaitForSeconds(.25f);
                yield return CheckMatches();
                yield return new WaitForSeconds(.25f);
                MatchFound = true;
                Highlighter.Instance.Activate();
            }
        }

        void HandleAllMatches()
        {
            HashSet<CoordinatesOffset> hexagonsToExplode = GetHexagonsToExplode();

            foreach (var hexagon in hexagonsToExplode)
            {
                Explode(hexagon);
            }
        }

        HashSet<CoordinatesOffset> GetHexagonsToExplode()
        {
            HashSet<CoordinatesOffset> hexagonsToExplode = new HashSet<CoordinatesOffset>();

            while (matches.Count != 0)
            {
                var group = matches.Dequeue();

                GameManager.Instance.ShowScore(group.Center, 3);

                hexagonsToExplode.Add(group.Hexagon1);
                hexagonsToExplode.Add(group.Hexagon2);
                hexagonsToExplode.Add(group.Hexagon3);
            }

            return hexagonsToExplode;
        }

        bool FindAllMatches()
        {
            bool matchFound = false;

            foreach (var group in GameManager.Instance.hexagonGroups)
            {
                var isMatch = CheckForMatch(group);

                if (isMatch)
                {
                    matchFound = true;
                    AddMatch(group);
                }
            }

            return matchFound;
        }

        void AddMatch(Group group)
        {
            matches.Enqueue(group);
        }

        IEnumerator ShiftAndFill()
        {
            yield return GridController.Instance.ShiftAndFill();
        }

        bool CheckForMatch(Group group)
        {
            var (hexagon1, hexagon2, hexagon3) = GridDatabase.Instance[group];
            return GameLogic.IsSameColor(hexagon1.GetComponent<Hexagon>(), hexagon2.GetComponent<Hexagon>(), hexagon3.GetComponent<Hexagon>());
        }

        void Explode(CoordinatesOffset coords)
        {
            var hex = GridDatabase.Instance[coords];
            GridDatabase.Instance.MarkAsDestroyed(coords);
            hex.GetComponent<Hexagon>().Explode();

            //Add Score
            GameManager.Instance.AddScore();
        }
    }
}
