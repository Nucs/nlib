Z.IconLibrary (v1.0.1)

=== Description ===
C# Icon Library add 3800 icons 16x16 and 32x32 to any kind of project (Web, WinForm, Etc.). 
All Icons have been designed by professional (FatCow Web Hosting).


=== Web Setup ===
1. Include Z.IconLibrary to your project reference
2. Add a new httpHandlers to your web.config (Required for II6-)
	<add path="*/z.axd" verb="*" type="Z.IconLibrary.IconResourceHandler" validate="false" />
3. Add a new handlers to your web.config (Required for II7+)
	<add name="Z.IconLibrary.RessourceHandler" verb="*" path="*/z.axd" preCondition="integratedMode" type="Z.IconLibrary.IconResourceHandler" /> 
4. Use "Page.AddIconStyleSheet();" to include embedded css class to your page header 
	or include the css file in your project and include it in your page.

Setup example and available method: https://ziconlibrary.codeplex.com/wikipage?title=Web%20Examples


=== WinForm Setup ===
1. Include Z.IconLibrary to your project reference

Setup example and available method: https://ziconlibrary.codeplex.com/wikipage?title=WinForm%20Examples



=== License ===
Code are under MIT License
http://ziconlibrary.codeplex.com/license
Copyright (c) 2014 Jonathan Magnan. All rights reserved.
http://jonathanmagnan.com

All icons are licensed under a Creative Commons Attribution 3.0 License.
http://creativecommons.org/licenses/by/3.0/us/
Copyright 2009-2013 FatCow Web Hosting. All rights reserved.
http://www.fatcow.com/free-icons