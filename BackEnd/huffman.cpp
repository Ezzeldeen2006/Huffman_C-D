#include<stdio.h>
#include<stdlib.h>
#include<string.h>



struct HuffmanNode
{
    char byte;
    int freq;
    HuffmanNode* left;
    HuffmanNode* right;

    HuffmanNode(char b, int f)
    {
        byte = b;
        freq = f;
        left = NULL;
        right = NULL;
    }
    HuffmanNode(int f, HuffmanNode* l, HuffmanNode* r)
    {
        byte = 0;
        freq = f;
        left = l;
        right = r;
    }
};

//Ezz
struct PriorityQ
{
    HuffmanNode* data;
    PriorityQ* next;

    PriorityQ(HuffmanNode* node)
    {
        data = node;
        next = NULL;
    }
};


struct Codes
{
    char* code;
    int length;
    bool assigned;

    Codes()
    {
        code = NULL;
        length = 0;
        assigned = false;
    }
    ~Codes()
    {
        if (code)
            delete[] code;
    }
};

struct Compression
{
    char ending[9];
    long long size;
    long long compressedSize;
    int uniqueBytes;
    int treeHeight;
    int paddingBits;

    Compression()
    {
        strcpy(ending, ".ece2103");
        size = 0;
        compressedSize = 0;
        uniqueBytes = 0;
        treeHeight = 0;
        paddingBits = 0;
    }
};
class TableOfCodes
{
    Codes codes[256];

public:
    void setCodes(char byte, char* codeLength)
    {
        if (codes[(unsigned char)byte].code)
            delete[] codes[(unsigned char)byte].code;

        int length = strlen(codeLength);
        codes[(unsigned char)byte].code = new char[length + 1];
        strcpy(codes[(unsigned char)byte].code, codeLength);
        codes[(unsigned char)byte].length = length;
        codes[(unsigned char)byte].assigned = true;
    }

    char* Getcodes(char byte)
    {
        if (codes[(unsigned char)byte].assigned)
            return codes[(unsigned char)byte].code;
        return NULL;
    }
};


class writeBits //from gpt
{
    FILE* file;
    char buffer;
    int bitsInBuffer;
    long long totalBitsWritten;
public:
    writeBits(FILE* a)
    {
        file = a;
        buffer = 0;
        bitsInBuffer = 0;
        totalBitsWritten = 0;
    }

    void writeBit(int bit)
    {
        buffer = (buffer << 1) | (bit & 1);
        bitsInBuffer++;
        totalBitsWritten++;

        if (bitsInBuffer == 8) {
            fwrite(&buffer, 1, 1, file);
            buffer = 0;
            bitsInBuffer = 0;
        }
    }

    void writebits(char* bits)
    {
        for (int i = 0; bits[i] != '\0'; i++)
        {
            writeBit(bits[i] - '0');
        }
    }

    void flush() {
        if (bitsInBuffer > 0)
        {
            buffer <<= (8 - bitsInBuffer);
            fwrite(&buffer, 1, 1, file);
            buffer = 0;
            bitsInBuffer = 0;
        }
    }

    long long getTotalBitsWritten() {
        return totalBitsWritten;
    }

    int getPaddingBits() {
        return bitsInBuffer > 0 ? (8 - bitsInBuffer) : 0;
    }
};


class BitReader
{
    FILE* file;
    char buffer;
    int bitsInBuffer;
    long long totalBitsRead;
    bool endOfFile;

public:
    BitReader(FILE* f)
    {
        file = f;
        buffer = 0;
        bitsInBuffer = 0;
        totalBitsRead = 0;
        endOfFile = false;
    }

    int readBit()
    {
        if (bitsInBuffer == 0)
        {
            if (fread(&buffer, 1, 1, file) != 1)
            {
                endOfFile = true;
                return -1;
            }
            bitsInBuffer = 8;
        }

        int bit = (buffer >> (bitsInBuffer - 1)) & 1;
        bitsInBuffer--;
        totalBitsRead++;
        return bit;
    }

    bool isEndOfFile()
    {
        return endOfFile;
    }

    long long getTotalBitsRead()
    {
        return totalBitsRead;
    }
};

class PriorityQueue
{
    PriorityQ* head;
    int size;
public:
    PriorityQueue()
    {
        head = NULL;
        size = 0;
    }

    ~PriorityQueue() {
        while (!isEmpty())
        {
            dequeue();
        }
    }

    bool isEmpty()
    {
        if (head == NULL)
            return true;
        return false;
    }

    int getsize()
    {
        return size;
    }

    void Enqueue(HuffmanNode* node)
    {
        PriorityQ* NewNode = new PriorityQ(node);
        if (isEmpty() || node->freq < head->data->freq)
        {
            NewNode->next = head;
            head = NewNode;
        }
        else
        {
            PriorityQ* current = head;
            while (current->next != NULL && current->next->data->freq <= node->freq)
            {
                current = current->next;
            }
            NewNode->next = current->next;
            current->next = NewNode;
        }
        size++;
    }

    HuffmanNode* dequeue()
    {
        if (isEmpty())
        {
            return NULL;
        }
        PriorityQ* tempNode = head;
        HuffmanNode* result = tempNode->data;

        head = head->next;
        delete tempNode;
        size--;
        return result;
    }
};


void generateCodes(HuffmanNode* root, TableOfCodes& table, char* code, int depth)
{
    if (root == NULL)
        return;

    if (root->left == NULL && root->right == NULL)
    {
        code[depth] = '\0';
        table.setCodes((char)root->byte, code);
        return;
    }

    if (root->left) {
        code[depth] = '0';
        generateCodes(root->left, table, code, depth + 1);
    }

    if (root->right) {
        code[depth] = '1';
        generateCodes(root->right, table, code, depth + 1);
    }
}


HuffmanNode* buildHuffmanTree(int* freq)
{
    PriorityQueue q;

    for (int i = 0; i < 256; i++)
    {
        if (freq[i] > 0)
        {
            HuffmanNode* newNode = new HuffmanNode((char)i, freq[i]);
            q.Enqueue(newNode);
        }
    }
    while (q.getsize() > 1)
    {
        HuffmanNode* left = q.dequeue();
        HuffmanNode* right = q.dequeue();

        int totalfreq = left->freq + right->freq;
        HuffmanNode* parent = new HuffmanNode(totalfreq, left, right);

        q.Enqueue(parent);
    }
    return q.dequeue();
}

void freeHuffmanTree(HuffmanNode* root)
{
    if (root == NULL)
        return;

    freeHuffmanTree(root->left);
    freeHuffmanTree(root->right);
    delete root;
}
int getTreeHeight(HuffmanNode* root)
{
    if (root == NULL)
        return 0;

    int leftHeight = getTreeHeight(root->left);
    int rightHeight = getTreeHeight(root->right);

    return (leftHeight > rightHeight) ? leftHeight + 1 : rightHeight + 1;
}

int* FrequencyTable(char* inputfile, int buffer)
{
    FILE* file = fopen(inputfile, "rb");
    if (!file)
        return NULL;

    int* frequency = new int[256]();

    char* bufer = new char[buffer];

    //from geeksforgeeks: how to use fread and what is its return type
    size_t bytesread;
    while ((bytesread = fread(bufer, sizeof(char), buffer, file)) > 0)
    {
        for (size_t i = 0; i < bytesread; i++)
            frequency[(unsigned char)bufer[i]]++;
    }

    delete[] bufer;
    fclose(file);
    return frequency;
}

double FileSize(char* filename)
{
    FILE* file = fopen(filename, "rb");
    if (!file) return 0;

    fseek(file, 0, SEEK_END);
    long size = ftell(file);
    fclose(file);
    return size;
}


void saveHuffmanTree(FILE* file, HuffmanNode* root)
{
    if (root == NULL) {
        fputc(0, file);
        return;
    }

    if (root->left == NULL && root->right == NULL) {
        fputc(1, file);
        fputc(root->byte, file);
        fwrite(&root->freq, sizeof(int), 1, file);
    }
    else {
        fputc(2, file);
        fwrite(&root->freq, sizeof(int), 1, file);
        saveHuffmanTree(file, root->left);
        saveHuffmanTree(file, root->right);
    }
}


HuffmanNode* loadHuffmanTree(FILE* file)
{
    int marker = fgetc(file);

    if (marker == 0) {
        return NULL;
    }
    else if (marker == 1) {
        char byte = fgetc(file);
        int freq;
        fread(&freq, sizeof(int), 1, file);
        return new HuffmanNode(byte, freq);
    }
    else if (marker == 2) {
        int freq;
        fread(&freq, sizeof(int), 1, file);
        HuffmanNode* left = loadHuffmanTree(file);
        HuffmanNode* right = loadHuffmanTree(file);
        return new HuffmanNode(freq, left, right);
    }

    return NULL;
}


bool performCompression(char* input, char* output, int bufferSize, TableOfCodes& codeTable, HuffmanNode* huffmanroot, int* freq, double originalSize)
{
    FILE* inFile = fopen(input, "rb");
    if (!inFile)
        return false;

    FILE* outFile = fopen(output, "wb");
    if (!outFile)
    {
        fclose(inFile);
        return false;
    }

    Compression a;
    a.size = (unsigned long long)originalSize;
    a.treeHeight = getTreeHeight(huffmanroot);

    for (int i = 0; i < 256; i++)
    {
        if (freq[i] > 0)
            a.uniqueBytes++;
    }

    //from geeksforgeeks: how to use fwrite
    fwrite(&a, sizeof(Compression), 1, outFile);
    saveHuffmanTree(outFile, huffmanroot);

    writeBits bitWriter(outFile);

    char* buffer = new char[bufferSize];
    size_t bytesRead;
    long long totalBytesProcessed = 0;
    while ((bytesRead = fread(buffer, 1, bufferSize, inFile)) > 0)
    {
        for (size_t i = 0; i < bytesRead; i++)
        {
            char byte = buffer[i];
            char* code = codeTable.Getcodes(byte);

            if (code)
            {
                bitWriter.writebits(code);
            }
            else
            {
                delete[] buffer;
                fclose(inFile);
                fclose(outFile);
                return false;
            }
        }
        totalBytesProcessed += bytesRead;
        if (totalBytesProcessed % (bufferSize * 10) == 0)
        {
            printf("\rProgress: %.2f%%", ((double)totalBytesProcessed / (double)originalSize) * 100.0);
            fflush(stdout);
        }
    }

    //from gpt: how to pad the remaining bits
    bitWriter.flush();

    long currentPos = ftell(outFile);
    a.compressedSize = currentPos;
    a.paddingBits = bitWriter.getPaddingBits();

    fseek(outFile, 0, SEEK_SET);
    fwrite(&a, sizeof(Compression), 1, outFile);

    delete[] buffer;
    fclose(inFile);
    fclose(outFile);

    return true;
}


bool compressFile(char* input, char* output, int bufferSize)
{
    FILE* testFile = fopen(input, "rb");
    TableOfCodes codeTable;
    HuffmanNode* root;

    if (!testFile)
        return false;
    fclose(testFile);

    double fileSize = FileSize(input);
    if (fileSize == 0)
        return false;

    int* freq = FrequencyTable(input, bufferSize);
    if (!freq)
        return false;

    int uniqueBytes = 0;
    for (int i = 0; i < 256; i++)
        if (freq[i] > 0) uniqueBytes++;

    if (uniqueBytes == 0)
    {
        delete[] freq;
        return false;
    }

    root = buildHuffmanTree(freq);
    if (root == NULL)
    {
        delete[] freq;
        return false;
    }

    int treeHeight = getTreeHeight(root);
    char* tempCode = new char[treeHeight + 1];
    generateCodes(root, codeTable, tempCode, 0);
    delete[] tempCode;

    bool compress = performCompression(input, output, bufferSize, codeTable, root, freq, fileSize);

    if (compress)
    {
        double compressedSize = FileSize(output);
        double compresssedpercantage = (1.0 - compressedSize / fileSize) * 100;
        printf("\nSuccess! Compression ratio: %.2f%%\n", compresssedpercantage);
    }

    freeHuffmanTree(root);
    delete[] freq;

    return compress;
}

char* addece2103(char* input)
{
    int length = strlen(input);
    char* output = new char[length + 10];
    strcpy(output, input);
    strcat(output, ".ece2103");
    return output;
}

bool decompressFile(char* inputFile, char* outputFile, int bufferSize) {
    FILE* in = fopen(inputFile, "rb");
    if (!in) return false;

    Compression header;
    fread(&header, sizeof(Compression), 1, in);

    if (strncmp(header.ending, ".ece2103", 7) != 0) {
        printf("Invalid compressed file format.\n");
        fclose(in);
        return false;
    }

    HuffmanNode* root = loadHuffmanTree(in);
    if (!root) {
        fclose(in);
        return false;
    }

    FILE* out = fopen(outputFile, "wb");
    if (!out) {
        fclose(in);
        freeHuffmanTree(root);
        return false;
    }

    BitReader reader(in);
    long long bytesWritten = 0;
    char* buffer = new char[bufferSize];

    while (bytesWritten < header.size) {
        size_t chunkSize = 0;

        while (chunkSize < (size_t)bufferSize && bytesWritten < header.size)
        {
            HuffmanNode* current = root;
            while (current->left || current->right)
            {
                int bit = reader.readBit();
                if (bit == -1) break;
                current = (bit == 0) ? current->left : current->right;
            }
            buffer[chunkSize++] = current->byte;
            bytesWritten++;
        }

        fwrite(buffer, 1, chunkSize, out);

        if (bytesWritten % 100000 == 0) {
            printf("\rDecompressed: %lld / %lld bytes", bytesWritten, header.size);
            fflush(stdout);
        }
    }

    printf("\nDecompression complete.\n");
    fclose(in);
    fclose(out);
    freeHuffmanTree(root);
    delete[] buffer;
    return true;
}


int main(int argc, char* argv[])
{
    bool compress = false;
    bool decompress = false;
    int bufferSize = 1024;
    char* inputFile = NULL;
    char* outputFile = NULL;

    if (argc == 1)
    {
        printf("Invalid argument. Press h for help.\n");
        return 0;
    }


    if ((argc == 2) && (strcmp(argv[1], "H") == 0 || strcmp(argv[1], "h") == 0))
    {
        printf("Syntax:\n");
        printf("COMPRESSION:  %s -b bufferSize -c input output \n", argv[0]);
        printf(" DECOMPRESSION: %s -b bufferSize -d input output \n", argv[0]);
        return 0;
    }

    if (strcmp(argv[1], "-b") == 0 && argc >= 6)
    {
        bufferSize = atoi(argv[2]);
        if (bufferSize <= 0)
        {
            printf("Invalid buffer size.\n");
            return 1;
        }

        if (strcmp(argv[3], "-c") == 0)
        {
            decompress = false;
            compress = true;
        }
        else if (strcmp(argv[3], "-d") == 0)
        {
            decompress = true;
            compress = false;
        }
        else {
            printf("after -b. Use -c for compression or -d for decompression.\n");
            return 0;
        }

        inputFile = argv[4];
        outputFile = argv[5];
    }

    else if (argc >= 4)
    {
        if (strcmp(argv[1], "compress") == 0)
        {
            decompress = false;
            compress = true;
        }
        else if (strcmp(argv[1], "decompress") == 0)
        {
            decompress = true;
            compress = false;
        }
        else
            return 0;


        inputFile = argv[2];
        outputFile = argv[3];
        if (argc >= 5)
        {
            bufferSize = atoi(argv[4]);
            if (bufferSize <= 0)
                return 1992;

        }
    }



    if (compress)
    {
        outputFile = addece2103(inputFile);
        bool result = compressFile(inputFile, outputFile, bufferSize);
        delete[] outputFile;
        return result ? 0 : 1;
    }

    else if (decompress)
    {
        return decompressFile(inputFile, outputFile, bufferSize) ? 0 : 1;
    }

    return 0;
}