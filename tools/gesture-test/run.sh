#!/bin/bash
# Feeds thousands of simulated wobbly finger strokes to the Trick Training recogniser
# (Assets/Scripts/MiniGames/GestureRecognizer.cs) and prints how often each shape is
# recognised. Needs only the .NET SDK. Run after any change to the recogniser.
# Baseline 12 Nov 2026: lines 100%, circle 99%, zigzag 97.5%, taps ignored 100%.
set -e
HERE="$(cd "$(dirname "$0")" && pwd)"; B="$(mktemp -d)"
D="$(dirname "$(readlink -f "$(which dotnet)")")"
CSC=$(ls "$D"/sdk/*/Roslyn/bincore/csc.dll | head -1); FW=$(ls -d "$D"/shared/Microsoft.NETCore.App/* | head -1)
REFS=$(for f in System.Runtime System.Console System.Collections System.Linq System.Private.CoreLib; do echo -n "-r:$FW/$f.dll "; done)
echo "{\"runtimeOptions\":{\"tfm\":\"net8.0\",\"framework\":{\"name\":\"Microsoft.NETCore.App\",\"version\":\"$(basename "$FW")\"}}}" > "$B/t.runtimeconfig.json"
dotnet "$CSC" -nologo -langversion:9.0 $REFS -out:"$B/t.dll" "$HERE/Shim.cs" "$HERE/Test.cs" "$HERE/../../unity-project/Assets/Scripts/MiniGames/GestureRecognizer.cs"
dotnet "$B/t.dll"; rm -rf "$B"
