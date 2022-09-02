using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;

// See https://aka.ms/new-console-template for more information
//string _trainDataPath =  @"C:\Users\Yohoo\Downloads\taxi-fair-train.csv";
string _testDataPath =  @"C:\Users\Yohoo\Downloads\taxi-fair-test.csv";
string _modelPath = @"C:\Users\Yohoo\Downloads\Model.zip";
//MLContext mlContext = new MLContext(seed: 0);
//var model = Train(mlContext, _trainDataPath);
//Evaluate(mlContext, model);

//TestSinglePrediction(mlContext, model);
//mlContext.Model.Save(model, null, _modelPath);
//DataViewSchema modelSchema;
//var context = new MLContext(seed: 0);
//ITransformer Trained = context.Model.Load(_modelPath, out modelSchema);
//TestSinglePrediction(context, Trained);
MLContext mlContext = new MLContext();
var onnxPredictionPipeline = GetPredictionPipeline(mlContext);
Console.ReadLine();

void Evaluate(MLContext mlContext, ITransformer model)
{

    IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(_testDataPath, hasHeader: true, separatorChar: ',');
    var predictions = model.Transform(dataView);
    var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
    Console.WriteLine();
    Console.WriteLine($"*************************************************");
    Console.WriteLine($"*       Model quality metrics evaluation         ");
    Console.WriteLine($"*------------------------------------------------");
    Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
    Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");

}
void TestSinglePrediction(MLContext mlContext, ITransformer model)
{
    var predictionFunction = mlContext.Model.CreatePredictionEngine<TaxiTrip, TaxiTripFarePrediction>(model);
    var taxiTripSample = new TaxiTrip()
    {
        VendorId = "VTS",
        RateCode = "1",
        PassengerCount = 1,
        TripTime = 1140,
        TripDistance = 3.75f,
        PaymentType = "CRD",
        FareAmount = 0 // To predict. Actual/Observed = 15.5
    };
    var prediction = predictionFunction.Predict(taxiTripSample);
    Console.WriteLine($"**********************************************************************");
    Console.WriteLine($"Predicted fare: {prediction.FareAmount:0.####}, actual fare: 15.5");
    Console.WriteLine($"**********************************************************************");
}
ITransformer GetPredictionPipeline(MLContext mlContext)
{

    var onnxPredictionPipeline =
    mlContext
        .Transforms
        .ApplyOnnxModel(
            outputColumnNames: new string[] { "last_hidden_state" },
            inputColumnNames: new string[]{"input_ids", "attention_mask"},
            @"C:\Users\Yohoo\Downloads\model.onnx");
    var emptyDv = mlContext.Data.LoadFromEnumerable(new OnnxInput[] { });

    return onnxPredictionPipeline.Fit(emptyDv);
}
ITransformer Train(MLContext mlContext, string dataPath)
{
    IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(dataPath, hasHeader: true, separatorChar: ',');
    var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount")
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName: "VendorId"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
        .Append(mlContext.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded", "PassengerCount", "TripDistance", "PaymentTypeEncoded"))
        .Append(mlContext.Regression.Trainers.FastTree());
    var model = pipeline.Fit(dataView);
    return model;
}
class TaxiTrip
{
    [LoadColumn(0)]
    public string VendorId;

    [LoadColumn(1)]
    public string RateCode;

    [LoadColumn(2)]
    public float PassengerCount;

    [LoadColumn(3)]
    public float TripTime;

    [LoadColumn(4)]
    public float TripDistance;

    [LoadColumn(5)]
    public string PaymentType;

    [LoadColumn(6)]
    public float FareAmount;
}
 class TaxiTripFarePrediction
{
    [ColumnName("Score")]
    public float FareAmount;
}


public class OnnxInput
{
    [ColumnName("input_ids")]
    public Int64[] Inputids { get; set; }

    [ColumnName("attention_mask")]
    public Int64[] RateCode { get; set; }

 
}
public class OnnxOutput
{
    [ColumnName("last_hidden_state")]
    public float[] last_hidden_state { get; set; }
}