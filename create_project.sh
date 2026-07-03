#!/bin/bash
# Create Unity project with URP template
UNITY_HUB="/Applications/Unity Hub.app/Contents/MacOS/Unity Hub"
UNITY_EDITOR="/Applications/Unity/Hub/Editor/6000.0.58f2/Unity.app/Contents/MacOS/Unity"
PROJECT_PATH="$(pwd)/HospitalRescueSimulator"

# Method 1: Using Unity Editor CLI to create project
"$UNITY_EDITOR" -createProject "$PROJECT_PATH" -logFile
echo "Exit code: $?"
