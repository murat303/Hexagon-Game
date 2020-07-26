using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hexagon
{
    public class GameController : MonoBehaviour
    {
        void Start()
        {
            GameManager.Instance.OnRotationCompleted += OnRotationCompleted;
        }

        void OnRotationCompleted(bool matchFound)
        {
            bool areTherePossibleMoves = AreTherePossibleMoves();

            if (false == areTherePossibleMoves)
                GameManager.Instance.GameOver();
        }

        bool AreTherePossibleMoves()
        {
            // To make a red explosion:
            // 1. 2 red must be connected (pair).
            // 2. A 3rd red must exist in the groups neighboring the group that contains the third hexagon of 2 red pair.

            foreach (var group in GameManager.Instance.hexagonGroups)
            {
                if (HasTwoColourPair(group, out var pairSpots, out var thirdSpot, out var foundColor))
                {
                    var (indirectNeighbors, directNeighbors) = FindNeighbors(thirdSpot, otherSpots: pairSpots);

                    foreach (var g in indirectNeighbors)
                    {
                        // indirect neighbors need at least one in order to qualify.
                        if (GameLogic.CountForColor(g, foundColor) > 0)
                        {
                            return true;
                        }
                    }

                    foreach (var g in directNeighbors)
                    {
                        // direct neighbors already have one due to sharing two spots. they need two in order to qualify.
                        if (GameLogic.CountForColor(g, foundColor) > 1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if there are any colours that exists two times in the <paramref name="group"/>.
        /// </summary>
        /// <param name="group">The group to check.</param>
        /// <param name="pairSpots">The spots where the color pair is located. (null if method returns false)</param>
        /// <param name="thirdSpot">The third commonSpot, i.e. the commonSpot that has a different color. (Hexagon3 if method returns false)</param>
        /// <param name="foundColor">The colour that exists 2 times in this group. (Color.Clear if method returns false)</param>
        bool HasTwoColourPair(Group group, out CoordinatesOffset[] pairSpots, out CoordinatesOffset thirdSpot, out Color foundColor)
        {
            var (ac, bc, cc) = GameLogic.GetColors(group);

            if (ac == bc)
            {
                thirdSpot = group.Hexagon3;
                foundColor = ac;
                pairSpots = new[] {group.Hexagon1, group.Hexagon2};
                return true;
            }

            if (bc == cc)
            {
                thirdSpot = group.Hexagon1;
                foundColor = bc;
                pairSpots = new[] {group.Hexagon2, group.Hexagon3};
                return true;
            }

            if (ac == cc)
            {
                thirdSpot = group.Hexagon2;
                foundColor = ac;
                pairSpots = new[] {group.Hexagon1, group.Hexagon3};
                return true;
            }

            thirdSpot = group.Hexagon3;
            foundColor = Color.clear;
            pairSpots = null;
            return false;
        }

        /// <summary>
        /// Returns the groups that contains the <paramref name="commonSpot"/>. Excludes <paramref name="otherSpots"/>.
        /// Direct neighbors: neighbors that share 1 spot from <paramref name="otherSpots"/> along with the <paramref name="commonSpot"/>.
        /// Indirect neighbors: neighbors that share only the <paramref name="commonSpot"/>.
        /// </summary>
        static (List<Group> indirectNeighbors, List<Group> directNeighbors) FindNeighbors(CoordinatesOffset commonSpot, CoordinatesOffset[] otherSpots)
        {
            var indirectNeighbors = new List<Group>();
            var directNeighbors = new List<Group>();

            foreach (var g in GameManager.Instance.hexagonGroups)
            {
                if (GameLogic.Contains(g, commonSpot))
                {
                    var otherSpotCount = otherSpots.Count(s => GameLogic.Contains(g, s));

                    switch (otherSpotCount)
                    {
                        case 0:
                            indirectNeighbors.Add(g);
                            break;

                        case 1:
                            directNeighbors.Add(g);
                            break;
                    }
                }
            }

            return (indirectNeighbors, directNeighbors);
        }
    }
}
