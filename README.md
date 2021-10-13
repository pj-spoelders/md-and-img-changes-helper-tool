# md-and-img-changes-helper-tool

A simple tool to facilitate image filename changes in a repository's markdown files.
## Purpose
The idea behind this tool: 

When you need to change image names in .md files this gets finicky. 

You got to alter them manually in your .md file and in the file system. 

It's a 2 step process and it's error prone and very 'not fun'.

This tool takes the finicky part out of it, makes it not error prone and reduces the manual labour involved to the minimum.

## Additional feature: finding 'floating' images.
The tool also lists 'floating' images not used in any of the .md files in the repository.

## Current state
100% Functional.

Needs error handling, some typical string handling stuff like trimming, maybe a lookup mechanism.


## Other features that could be added
- Finding floating img links in .md files.
- A refresh overview function and an abort function
