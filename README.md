# Hello!
In unity we mostly use audio as PlayOneShot most of the time. As we all know, PlayOneShot takes file name as params. But managing file name via string => YUCK!

This project turns all your audio files in Enum as part of a scriptable object!

# Usage: 
(Make sure you have at least one audio file in project!)

## 1. Go To Window/EzAudioManager
## 2. Click the big button
## 3. It will generate a Scriptable Object in Resources/EzAudioBook.asset 
=== This should create all the necessary files. Check if all sounds came into your scriptable object.
## 4. In your scene, Add EzAudioManager/EzAudioSystem.prefab
## 5. In your script, just write

`EzAudio.EzAudioSystem.instance.PlayClip(EzAudioFiles.XIT);`


### Common Concerns:

1. If filename cannot be turned as an identifier, then it will have `INVALID_NAME_X` where x will be a number ofcourse. 

If you want to investigate further, go to `EzAudioManager/EzAudioFiles.cs`. You will see which files is invalidated due to bad name. 
(You are welcome! :P )

2. If same filename found in multiple places, same thing will happen, it will be marked as `INVALID_NAME_X`
3. Spaces in file names will be replaced by underscore

## Improvement Points:
1. Optimize for Background and other musics, which are not necessarily played with PlayOneShot function.


### License & Policy
### Do whatever you want with it. I hope it will help you in your project!

Thanks!

My twitter `@game_auz`

Thanks to my company **Ulka Games** who give me enough freedom to publish these kind of things.
