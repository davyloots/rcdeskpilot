// Bonsai.Native.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#include "Bonsai.Native.h"

#include <Include/vorbis/codec.h>
#include <Include/vorbis/vorbisfile.h>

#ifdef _MANAGED
#pragma managed(push, off)
#endif
/*
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}
*/

BONSAINATIVE_API int DecodeVorbisFile(
	char* fileName, 
	char* buffer, 
	int bufferSize, 
	int& samplesPerSec,
	int& channels)
{
	FILE *file;
    file = fopen(fileName, "rb");
    if (file == 0)
    {
        // Cleanup
        return -1;
    }
    OggVorbis_File vorbisFile;
    ov_open(file, &vorbisFile, NULL, 0);

    vorbis_info *vi = ov_info(&vorbisFile, -1);
    
	samplesPerSec = (int) vi->rate;
	channels = (int) vi->channels;

	// read in the data
	int retval = 1;
    int pos = 0;
    int sec = 0;
    while (retval && pos < (int)bufferSize)
    {
        retval = ov_read(&vorbisFile, (char*)(buffer) + pos, bufferSize - pos, 0, 2, 1, &sec);
        pos += retval;
    }

    // fill the remainder with silence
    if (pos == 0)
    {
        // Wav is blank, so just fill with silence
        FillMemory((char*) buffer, 
                   bufferSize, 
                   0);
    }
    else if (pos < bufferSize)
    {
        FillMemory((char*) buffer + pos, 
                    bufferSize - pos, 
                    0);
    }

	// close up
	ov_clear(&vorbisFile);
    fclose(file);


	return 0;
}


BONSAINATIVE_API int DecodedVorbisSize(
	char* fileName,
	int& bufferSize)
{
	FILE *file;
    file = fopen(fileName, "rb");
    if (file == 0)
    {
        // Cleanup
        return -1;
    }
    OggVorbis_File vorbisFile;
    ov_open(file, &vorbisFile, NULL, 0);

    vorbis_info *vi = ov_info(&vorbisFile, -1);

    // Get the size of the VorbisFile so we can set the size of the buffer
    bufferSize = (int) ov_pcm_total(&vorbisFile, -1) * 2 * vi->channels;

	// close up
	ov_clear(&vorbisFile);
    fclose(file);

	return 0;
}
