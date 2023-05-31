# Disclaimer
(This is a personal student project made in a few weeks, this isn't intended for use and many features are still missing)

# Node Editor 

A Runtime Visual Scripting Node Editor made in Unity

Inspired by the Blueprint Editor in Unreal Engine, this editor can execute a series of pre-defined functions (made in C#) at runtime using nodes

## Features
- Draggable Nodes
- Port Types (Exec, Integer & String have currently been tested) 
- Node Compiling from the Unity Editor to use at Runtime

### How it works

The Entry Node is the entry point of the instructions. This node has an Exec Output port. Exec ports are single-directional, unique port type that defines the order of instructions in the graph.
