/* Form1.h */

//
// Mandelbrot generation with managed Visual C++
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//
#pragma once
#include "Mandelbrot.h"

namespace SeqMandelbrot
{
	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Summary for Form1
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class Form1 : public Form
	{
	public:
		Form1(void);

	protected:
		~Form1();

	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::Button^  _goButton;
	private: System::Windows::Forms::Panel^  _imagePanel;
  private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::Label^  _timeLabel;
  private: System::Windows::Forms::Label^  _versionLabel;
  private: System::Windows::Forms::Label^  _threadsLabel;
  private: System::Windows::Forms::Label^  label3;
	private: System::ComponentModel::Container ^components;

  private: void OnProgress(Object ^sender, ProgressChangedEventArgs ^e);
  private: void OnComplete(Object ^sender, RunWorkerCompletedEventArgs ^e);
	private: void _imagePanel_Paint(Object ^sender, PaintEventArgs ^e);
  private: void GoButton_Click(System::Object^  sender, System::EventArgs^  e);
  private: void Form1_Load(System::Object^  sender, System::EventArgs^  e);
  private: void Form1_FormClosed(System::Object^  sender, System::EventArgs^  e);

  private: Bitmap ^_image;
	private: bool    _running;
	private: Mandelbrot ^_mandelbrot;
  private: BackgroundWorker ^_worker;
  
#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
      System::ComponentModel::ComponentResourceManager^  resources = (gcnew System::ComponentModel::ComponentResourceManager(Form1::typeid));
      this->label1 = (gcnew System::Windows::Forms::Label());
      this->_goButton = (gcnew System::Windows::Forms::Button());
      this->_imagePanel = (gcnew System::Windows::Forms::Panel());
      this->label2 = (gcnew System::Windows::Forms::Label());
      this->_timeLabel = (gcnew System::Windows::Forms::Label());
      this->_versionLabel = (gcnew System::Windows::Forms::Label());
      this->_threadsLabel = (gcnew System::Windows::Forms::Label());
      this->label3 = (gcnew System::Windows::Forms::Label());
      this->SuspendLayout();
      // 
      // label1
      // 
      this->label1->AutoSize = true;
      this->label1->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
        static_cast<System::Byte>(0)));
      this->label1->Location = System::Drawing::Point(22, 13);
      this->label1->Name = L"label1";
      this->label1->Size = System::Drawing::Size(75, 20);
      this->label1->TabIndex = 0;
      this->label1->Text = L"Version:";
      // 
      // _goButton
      // 
      this->_goButton->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
        static_cast<System::Byte>(0)));
      this->_goButton->Location = System::Drawing::Point(510, 13);
      this->_goButton->Name = L"_goButton";
      this->_goButton->Size = System::Drawing::Size(104, 49);
      this->_goButton->TabIndex = 5;
      this->_goButton->Text = L"&Go";
      this->_goButton->UseVisualStyleBackColor = true;
      this->_goButton->Click += gcnew System::EventHandler(this, &Form1::GoButton_Click);
      // 
      // _imagePanel
      // 
      this->_imagePanel->BackColor = System::Drawing::Color::White;
      this->_imagePanel->Location = System::Drawing::Point(14, 78);
      this->_imagePanel->Name = L"_imagePanel";
      this->_imagePanel->Size = System::Drawing::Size(600, 600);
      this->_imagePanel->TabIndex = 6;
      // 
      // label2
      // 
      this->label2->AutoSize = true;
      this->label2->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
        static_cast<System::Byte>(0)));
      this->label2->Location = System::Drawing::Point(22, 42);
      this->label2->Name = L"label2";
      this->label2->Size = System::Drawing::Size(52, 20);
      this->label2->TabIndex = 3;
      this->label2->Text = L"Time:";
      // 
      // _timeLabel
      // 
      this->_timeLabel->AutoSize = true;
      this->_timeLabel->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
        static_cast<System::Byte>(0)));
      this->_timeLabel->Location = System::Drawing::Point(105, 42);
      this->_timeLabel->Name = L"_timeLabel";
      this->_timeLabel->Size = System::Drawing::Size(18, 20);
      this->_timeLabel->TabIndex = 3;
      this->_timeLabel->Text = L"\?";
      // 
      // _versionLabel
      // 
      this->_versionLabel->AutoSize = true;
      this->_versionLabel->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
        static_cast<System::Byte>(0)));
      this->_versionLabel->Location = System::Drawing::Point(105, 13);
      this->_versionLabel->Name = L"_versionLabel";
      this->_versionLabel->Size = System::Drawing::Size(18, 20);
      this->_versionLabel->TabIndex = 7;
      this->_versionLabel->Text = L"\?";
      // 
      // _threadsLabel
      // 
      this->_threadsLabel->AutoSize = true;
      this->_threadsLabel->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
        static_cast<System::Byte>(0)));
      this->_threadsLabel->Location = System::Drawing::Point(106, 690);
      this->_threadsLabel->Name = L"_threadsLabel";
      this->_threadsLabel->Size = System::Drawing::Size(18, 20);
      this->_threadsLabel->TabIndex = 9;
      this->_threadsLabel->Text = L"\?";
      // 
      // label3
      // 
      this->label3->AutoSize = true;
      this->label3->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
        static_cast<System::Byte>(0)));
      this->label3->Location = System::Drawing::Point(11, 690);
      this->label3->Name = L"label3";
      this->label3->Size = System::Drawing::Size(92, 20);
      this->label3->TabIndex = 8;
      this->label3->Text = L"Thread IDs:";
      // 
      // Form1
      // 
      this->AcceptButton = this->_goButton;
      this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
      this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
      this->BackColor = System::Drawing::Color::PaleGreen;
      this->ClientSize = System::Drawing::Size(628, 722);
      this->Controls->Add(this->_threadsLabel);
      this->Controls->Add(this->label3);
      this->Controls->Add(this->_versionLabel);
      this->Controls->Add(this->_imagePanel);
      this->Controls->Add(this->_goButton);
      this->Controls->Add(this->_timeLabel);
      this->Controls->Add(this->label2);
      this->Controls->Add(this->label1);
      this->FormBorderStyle = System::Windows::Forms::FormBorderStyle::Fixed3D;
      this->Icon = (cli::safe_cast<System::Drawing::Icon^  >(resources->GetObject(L"$this.Icon")));
      this->MaximizeBox = false;
      this->Name = L"Form1";
      this->StartPosition = System::Windows::Forms::FormStartPosition::CenterScreen;
      this->Text = L"Sequential Mandelbrot";
      this->Load += gcnew System::EventHandler(this, &Form1::Form1_Load);
      this->Closed += gcnew System::EventHandler(this, &Form1::Form1_FormClosed);
      this->ResumeLayout(false);
      this->PerformLayout();

    }
#pragma endregion
		
};  //class Form1
}

