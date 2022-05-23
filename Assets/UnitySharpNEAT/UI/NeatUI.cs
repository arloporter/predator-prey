/*
------------------------------------------------------------------
  This file is part of UnitySharpNEAT 
  Copyright 2020, Florian Wolf
  https://github.com/flo-wolf/UnitySharpNEAT
------------------------------------------------------------------
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.DistanceMetrics;
using SharpNeat.Domains;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using SharpNeat.SpeciationStrategies;
namespace UnitySharpNEAT
{
    public class NeatUI : MonoBehaviour
    {
        [SerializeField] 
        private NeatSupervisor _neatSupervisor;
        public uint CurrentGeneration { get; private set; }
        public double CurrentBestFitness { get; private set; }
        public NeatEvolutionAlgorithm<NeatGenome> EvolutionAlgorithm { get; private set; }
        public Experiment Experiment { get; private set; }
        /// <summary>
        /// Display simple Onscreen buttons for quickly accessing ceratain lifecycle funtions and to display generation info.
        /// </summary>
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 110, 40), "Start EA"))
            {
                
                _neatSupervisor.StartEvolution();
                Experiment.LoadPopulation();
            }
            if (GUI.Button(new Rect(10, 60, 110, 40), "Stop + save EA"))
            {
                _neatSupervisor.StopEvolution();
                Experiment.SavePopulation(EvolutionAlgorithm.GenomeList);
                Experiment.SaveChampion(EvolutionAlgorithm.CurrentChampGenome);
            }
            if (GUI.Button(new Rect(10, 110, 110, 40), "Run best"))
            {
                _neatSupervisor.RunBest();
            }
            if (GUI.Button(new Rect(10, 160, 110, 40), "Delete Saves"))
            {
                ExperimentIO.DeleteAllSaveFiles(_neatSupervisor.Experiment);
            }

            GUI.Button(new Rect(10, Screen.height - 70, 110, 60), string.Format("Generation: {0}\nFitness: {1:0.00}", _neatSupervisor.CurrentGeneration, _neatSupervisor.CurrentBestFitness));
        }
    }
}
