using System;
using System.Collections.Generic;

namespace EvidenceManager2 {
    class Program {
        static string[] Evidences;
        static bool[] PosEvidences;
        static bool[] Impossible;
        static bool[] PossibleGhosts;
        static int[] Confirmed;
        static int PosGhosts;
        static int[] EvidenceOccurances;
        static Ghost[] GhostTypes;
        static void Main(string[] args) {
            Console.Title = "EvidenceManager2";

            Evidences = new string[7] {"EMF 5", "Spirit Box", "Fingerprints", "Ghost Orb", "Ghost Writing", "Freezing Temperatures", "D.O.T.S."};
            PosEvidences = new bool[7] {false, false, false, false, false, false, false};
            Impossible = new bool[7] {false, false, false, false, false, false, false};
            PossibleGhosts = new bool[16];
            Confirmed = new int[3] {-1, -1, -1};
            PosGhosts = 0;
            EvidenceOccurances = new int[7] {0, 0, 0, 0, 0, 0, 0};
            
            GhostTypes = new Ghost[16];

            GhostTypes[0] = new Ghost("Banshee", 2, 3, 6);
            GhostTypes[1] = new Ghost("Demon", 2, 4, 5);
            GhostTypes[2] = new Ghost("Goryo", 0, 2, 6);
            GhostTypes[3] = new Ghost("Hantu", 2, 3, 5);
            GhostTypes[4] = new Ghost("Jinn", 0, 2, 5);
            GhostTypes[5] = new Ghost("Mare", 1, 3, 4);
            GhostTypes[6] = new Ghost("Myling", 0, 2, 4);
            GhostTypes[7] = new Ghost("Oni", 0, 5, 6);
            GhostTypes[8] = new Ghost("Phantom", 1, 2, 6);
            GhostTypes[9] = new Ghost("Poltergeist", 1, 2, 4);
            GhostTypes[10] = new Ghost("Revenant", 3, 4, 5);
            GhostTypes[11] = new Ghost("Shade", 0, 4, 5);
            GhostTypes[12] = new Ghost("Spirit", 0, 1, 4);
            GhostTypes[13] = new Ghost("Wraith", 0, 1, 6);
            GhostTypes[14] = new Ghost("Yokai", 1, 3, 6);
            GhostTypes[15] = new Ghost("Yurei", 3, 5, 6);

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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            MakeLine();

            Console.Write("Confirmed evidence: ");
            for (int e = 0; e < Confirmed.Length; e++) {
                if (Confirmed[e] != -1) {
                    Console.Write(Evidences[Confirmed[e]] + ", ");
                }
            }
            Console.WriteLine("\b\b");
            Console.Write("Impossible evidence: ");
            for (int e = 0; e < Impossible.Length; e++) {
                if (Impossible[e]) {
                    Console.Write(Evidences[e] + ", ");
                }
            }
            Console.WriteLine("\b\b");

            GetGhosts();
            GetEviAmount();

            MakeLine();
            Console.WriteLine("Possible ghosts:");
            for(int i = 0; i < PossibleGhosts.Length; i++) {
                //If the ghost is possible, print it along with non-confirmed evidence
                if(PossibleGhosts[i]) {
                    //Print ghost.name + ": "
                    Console.Write(GhostTypes[i].name + ":");
                    int nameLength = GhostTypes[i].name.Length + 1;
                    int curIndent = (int)Math.Ceiling((nameLength/8f));
                    int tarIndent = 2;
                    for(int d = 0; d < tarIndent-curIndent; d++) {
                        Console.Write("\t");
                    }
                    
                    //Loop through every evidence of current ghost and print non confirmed
                    for(int e = 0; e < GhostTypes[i].values.Length; e++) {
                        if(!InArr(Confirmed, GhostTypes[i].values[e])) {
                            Console.Write(Evidences[GhostTypes[i].values[e]] + ",\t");
                        }
                    }
                    Console.WriteLine("\b\b");
                }
            }

            MakeLine();
            Console.WriteLine("Possible remaining evidence:");
            //Find highest and lowest chance
            float HChance = 0;
            float LChance = 1;
            for(int i = 0; i < PosEvidences.Length; i++) {
                if(PosEvidences[i]) {
                    float Chance = (float)EvidenceOccurances[i] / (float)PosGhosts;
                    if(Chance > HChance) {
                        HChance = Chance;
                    } else if(Chance < LChance) {
                        LChance = Chance;
                    }
                }
            }
            for(int i = 0; i < PosEvidences.Length; i++) {
                if(PosEvidences[i]) {
                    float Chance = (float)EvidenceOccurances[i] / (float)PosGhosts;

                    if(Chance <= HChance) {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    if(Chance <= lerp(LChance, HChance, 0.67f)) {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    if(Chance < lerp(LChance, HChance, 0.33f)) {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.WriteLine((i+1).ToString() + ". " + Evidences[i] + " " + EvidenceOccurances[i] + "/" + PosGhosts);
                }
            }
            Console.ResetColor();
        }

        static void GetGhosts() {
            for (int b = 0; b < PossibleGhosts.Length; b++) {
                PossibleGhosts[b] = true;
            }
            for (int i = 0; i < GhostTypes.Length; i++) {
                for (int b = 0; b < GhostTypes[i].values.Length; b++) {
                    if (Impossible[GhostTypes[i].values[b]]) {
                        PossibleGhosts[i] = false;
                    }
                }
            }
            for (int e = 0; e < PossibleGhosts.Length; e++) {
                if (PossibleGhosts[e]) {
                    for (int c = 0; c < Confirmed.Length; c++) {
                        if (Confirmed[c] != -1) {
                            bool hasEvi = false;
                            for (int g = 0; g < GhostTypes.Length; g++) {
                                if (GhostTypes[e].values[g] == Confirmed[c]) {
                                    hasEvi = true;
                                    break;
                                }
                            }

                            if (!hasEvi) {
                                PossibleGhosts[e] = false;
                                break;
                            }
                        }
                    }
                }
            }

            PosGhosts = 0;
            for (int i = 0; i < PossibleGhosts.Length; i++) {
                if (PossibleGhosts[i]) {
                    PosGhosts++;
                }
            }
        }

        static void GetEviAmount() {
            for(int i = 0; i < EvidenceOccurances.Length; i++) {
                EvidenceOccurances[i] = 0;
            }
            for(int i = 0; i < PosEvidences.Length; i++) {
                PosEvidences[i] = false;
            }

            for(int i = 0; i < PossibleGhosts.Length; i++) {
                if(PossibleGhosts[i]) {
                    for(int j = 0; j < GhostTypes[i].values.Length; j++) {
                        EvidenceOccurances[GhostTypes[i].values[j]]++;
                        PosEvidences[GhostTypes[i].values[j]] = true;
                    }
                }
            }
            for(int c = 0; c < Confirmed.Length; c++) {
                if(Confirmed[c] != -1) {
                    PosEvidences[Confirmed[c]] = false;
                }
            }
        }

        static void MakeLine() {
            for (int i = 0; i < 120; i++) {
                Console.Write("\u2550");
            }
        }

        static bool InArr(int[] arr, int objec) {
            for (int e = 0; e < arr.Length; e++) {
                if (arr[e] == objec) {
                    return true;
                }
            }
            return false;
        }

        static float lerp(float a, float b, float t) {
            return (a + t*(b-a));
        }
    }
}
