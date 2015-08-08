#include <string.h>
#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    if (argc < 2) // Do we have an input file?
    {
        printf("No input file.");
        exit(1);
    }

    FILE *file;
    file = fopen(argv[1], "r");
    if (file)
    {
        // Get file size and seek to beginning.
        fseek(file, 0, SEEK_END);
        size_t length = (size_t) ftell(file);
        fseek(file, 0, SEEK_SET);

        // Read file into buffer.
        char *buffer;
        buffer = malloc(length);
        if (buffer)
        {
            fread(buffer, 1, length, file);
            fclose(file);
            buffer[length] = 0; // Nul-terminate string.
            printf(buffer); // TODO: A simple cat for now, develop.
        }
        else
        {
            printf("Unable to allocate memory.");
            exit(1);
        }
    }
    else
    {
        printf("Unable to open input file.");
        exit(1);
    }

    return 0;
}
