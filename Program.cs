using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvidenceManager {
    class Program {
        static void Main(string[] args) {
            string[] Evidences = {"EMF 5", "Spirit Box", "Fingerprints", "Ghost Orb", "Ghost Writing", "Freezing Temperatures", "D.O.T.S."};
            bool[] PosEvidences = {false, false, false, false, false, false, false};
            bool[] Impossible = {false, false, false, false, false, false, false};
            bool[] PossibleGhosts;
            int[] Confirmed = {-1, -1, -1};
            int PosGhosts = 0;
            int[] EvidenceOccurances = {0, 0, 0, 0, 0, 0, 0};
            
            var GhostTypes = new List<(string GhostName, List<int> EvidenceList)>();

            GhostTypes.Add(("Banshee", new List<int>{2, 3, 6}));
            GhostTypes.Add(("Demon", new List<int>{2, 4, 5}));
            GhostTypes.Add(("Goryo", new List<int>{0, 2, 6}));
            GhostTypes.Add(("Hantu", new List<int>{2, 3, 5}));
            GhostTypes.Add(("Jinn", new List<int>{0, 2, 5}));
            GhostTypes.Add(("Mare", new List<int>{1, 3, 4}));
            GhostTypes.Add(("Myling", new List<int>{0, 2, 4}));
            GhostTypes.Add(("Oni", new List<int>{0, 5, 6}));
            GhostTypes.Add(("Phantom", new List<int>{1, 2, 6}));
            GhostTypes.Add(("Poltergeist", new List<int>{1, 2, 4}));
            GhostTypes.Add(("Revenant", new List<int>{3, 4, 5}));
            GhostTypes.Add(("Shade", new List<int>{0, 4, 5}));
            GhostTypes.Add(("Spirit", new List<int>{0, 1, 4}));
            GhostTypes.Add(("Wraith", new List<int>{0, 1, 6}));
            GhostTypes.Add(("Yokai", new List<int>{1, 3, 6}));
            GhostTypes.Add(("Yurei", new List<int>{3, 5, 6}));

            bool isInvalid = false;
            while (true) {
                DrawGUI();

                string Input;
                if (!isInvalid) {
                    Input = Console.ReadLine();
                } else {
                    Console.Write("That input is invalid!\n");
                    Input = Console.ReadLine();
                }

                int NewEvi = 0;
                bool success = int.TryParse(Input, out NewEvi);
                if (success) {
                    if (NewEvi >= 1 && NewEvi <= Evidences.Length) {
                        isInvalid = false;
                        bool deleted = false;
                        for (int c = 0; c < Confirmed.Length; c++) {
                            if (Confirmed[c] == NewEvi-1) {
                                Confirmed[c] = -1;
                                deleted = true;
                                break;
                            }
                        }
                        for (int c = 0; c < Confirmed.Length; c++) {
                            if (!deleted && Confirmed[c] == -1) {
                                Confirmed[c] = NewEvi-1;
                                break;
                            }
                        }
                    } else if (NewEvi <= -1 && NewEvi >= -Evidences.Length) {
                        isInvalid = false;
                        NewEvi = Math.Abs(NewEvi)-1;
                        if(Impossible[NewEvi]) {
                            Impossible[NewEvi] = false;
                        } else {
                            Impossible[NewEvi] = true;
                        }
                    }
                } else if(Input == "restart") {
                    isInvalid = false;
                    for(int c = 0; c < Confirmed.Length; c++) {
                        Confirmed[c] = -1;
                    }
                    for(int c = 0; c < Impossible.Length; c++) {
                        Impossible[c] = false;
                    }
                } else {
                    isInvalid = true;
                }
            }
        }
        
        static void DrawGUI() {
            Console.Clear();
            MakeLine();
        }

        static void MakeLine() {
            for (int i = 0; i < 256; i++) {
                int Line = i;
                Console.Write((char)Line);
            }
            Console.Write("\n");
        }
    }
}
