#include "stdafx.h"
#include <cv.h>
#include <highgui.h>


void main(void)
{
	IplImage *obrazek=cvLoadImage("pomiar.jpeg"); //wczytanie obrazu znajduj�cego si� w bie��cym katalogu programu
	cvSmooth( obrazek, obrazek, CV_GAUSSIAN, 8, 8 );
	cvShowImage("CannyTest",obrazek);               //wy�wietlenie wczytanego obrazu
	 
	cvWaitKey();                                  //wcisni�cie dowolnego klawisza ko�czy dzia�anie programu
}