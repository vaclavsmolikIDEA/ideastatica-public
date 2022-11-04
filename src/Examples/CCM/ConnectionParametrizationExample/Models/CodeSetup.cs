namespace ConnectionParametrizationExample.Models
{
	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class CodeSetup
	{
		public SteelSetup steelSetup { get; set; }
		public object concreteSetup { get; set; }
		public bool stopAtLimitStrain { get; set; }
		public int weldEvaluationData { get; set; }
		public bool checkDetailing { get; set; }
		public int applyConeBreakoutCheck { get; set; }
		public double pretensionForceFpc { get; set; }
		public double gammaInst { get; set; }
		public double gammaC { get; set; }
		public double gammaM3 { get; set; }
		public int anchorLengthForStiffness { get; set; }
		public double jointBetaFactor { get; set; }
		public double effectiveAreaStressCoeff { get; set; }
		public double effectiveAreaStressCoeffAISC { get; set; }
		public double frictionCoefficient { get; set; }
		public double limitPlasticStrain { get; set; }
		public double limitDeformation { get; set; }
		public bool limitDeformationCheck { get; set; }
		public bool analysisGNL { get; set; }
		public double warnPlasticStrain { get; set; }
		public double warnCheckLevel { get; set; }
		public double optimalCheckLevel { get; set; }
		public double distanceBetweenBolts { get; set; }
		public double distanceDiameterBetweenBP { get; set; }
		public double distanceBetweenBoltsEdge { get; set; }
		public double bearingAngle { get; set; }
		public double decreasingFtrd { get; set; }
		public bool bracedSystem { get; set; }
		public bool bearingCheck { get; set; }
		public bool applyBetapInfluence { get; set; }
		public double memberLengthRatio { get; set; }
		public int divisionOfSurfaceOfCHS { get; set; }
		public int divisionOfArcsOfRHS { get; set; }
		public int numElement { get; set; }
		public int numberIterations { get; set; }
		public int mdiv { get; set; }
		public double minSize { get; set; }
		public double maxSize { get; set; }
		public int numElementRhs { get; set; }
		public bool rigidBP { get; set; }
		public double alphaCC { get; set; }
		public bool crackedConcrete { get; set; }
		public bool developedFillers { get; set; }
		public bool deformationBoltHole { get; set; }
		public double extensionLengthRationOpenSections { get; set; }
		public double extensionLengthRationCloseSections { get; set; }
		public double factorPreloadBolt { get; set; }
		public bool baseMetalCapacity { get; set; }
		public bool applyBearingCheck { get; set; }
		public double frictionCoefficientPbolt { get; set; }
		public int crtCompCheckIS { get; set; }
		public double boltMaxGripLengthCoeff { get; set; }
		public double fatigueSectionOffset { get; set; }
		public double condensedElementLengthFactor { get; set; }
		public double gammaMu { get; set; }
	}

	public class SteelSetup
	{
		public double reductionFactorTension { get; set; }
		public double reductionFactorShear { get; set; }
		public double boltTensileShear_Omega { get; set; }
		public double boltTensileShearCombined_Omega { get; set; }
		public double boltBearing_Omega { get; set; }
		public double filletWelds_Omega { get; set; }
		public double boltTensileShear_Phi { get; set; }
		public double boltTensileShearCombined_Phi { get; set; }
		public double boltBearing_Phi { get; set; }
		public double filletWelds_Phi { get; set; }
		public double materialFy_Omega { get; set; }
		public double materialFy_Phi { get; set; }
		public double boltSlipRes_Phi { get; set; }
		public double boltSlipRes_Omega { get; set; }
	}


}
