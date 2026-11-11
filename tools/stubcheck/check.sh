#!/bin/bash
# Compile-checks every game and editor script WITHOUT Unity, against hand-written
# stand-ins for the Unity API (UnityEngine.cs / UnityEditor.cs in this folder).
# It proves the C# is syntactically and type-correct; it cannot prove Unity behaves
# as the stubs assume. Needs only the .NET SDK (dotnet). Run:  tools/stubcheck/check.sh
set -e
HERE="$(cd "$(dirname "$0")" && pwd)"
S="$HERE/../../unity-project/Assets/Scripts"
B="$(mktemp -d)"
CSC=$(ls "$(dirname "$(readlink -f "$(which dotnet)")")"/sdk/*/Roslyn/bincore/csc.dll | head -1)
FW=$(ls -d "$(dirname "$(readlink -f "$(which dotnet)")")"/shared/Microsoft.NETCore.App/* | head -1)
REFS=$(for f in "$FW"/*.dll; do case "$f" in *Native*|*mscordaccore*|*mscordbi*|*coreclr*|*clrjit*|*hostpolicy*|*clretwrc*|*clrgc*|*msquic*) ;; *) echo -n "-r:$f ";; esac; done)
OPT="-nologo -target:library -langversion:9.0 -nowarn:CS0169,CS0649,CS0414,CS0067"
dotnet "$CSC" $OPT $REFS -out:"$B/stubs.dll" "$HERE/UnityEngine.cs" "$HERE/UnityEditor.cs"
dotnet "$CSC" $OPT $REFS -r:"$B/stubs.dll" -out:"$B/game.dll" $(find "$S" -name '*.cs' -not -path '*/Editor/*')
dotnet "$CSC" $OPT $REFS -r:"$B/stubs.dll" -r:"$B/game.dll" -out:"$B/editor.dll" $(find "$S" -path '*/Editor/*.cs')
rm -rf "$B"
echo "ALL_OK — $(find "$S" -name '*.cs' | wc -l) scripts compile"
