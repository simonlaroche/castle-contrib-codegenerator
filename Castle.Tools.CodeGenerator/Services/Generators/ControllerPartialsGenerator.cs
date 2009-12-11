// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Castle.Tools.CodeGenerator.CodeDom;
using Castle.Tools.CodeGenerator.External;

namespace Castle.Tools.CodeGenerator.Services.Generators
{
	using System.CodeDom;
	using Model.TreeNodes;
	using Generators;

	public class ControllerPartialsGenerator : AbstractGenerator
	{
		const string CodeGeneratorServicesField = "codeGeneratorServices";
		const string EngineContextField = "Context";
		const string RoutesField = "routes";
		const string RoutesProperty = "Routes";
		const string SiteMapField = "siteMap";
		const string SiteMapProperty = "SiteMap";

		public ControllerPartialsGenerator(ILogger logger, ISourceGenerator source, INamingService naming,
		                                   string targetNamespace, string serviceType)
			: base(logger, source, naming, targetNamespace, serviceType)
		{
		}

		public override void Visit(ControllerTreeNode node)
		{
			var type = source.GenerateTypeDeclaration(node.Namespace, node.Name);
			var servicesField = source.NewField(CodeGeneratorServicesField, typeof (ICodeGeneratorServices).FullName);
			var rootAreaNodeType = @namespace + ".RootAreaNode";
			var routesNodeType = @namespace + ".Routes";

			servicesField.InitExpression = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(NullCodeGeneratorServices)), "Instance");

			type.Members.Add(servicesField);
			type.Members.Add(source.NewField(SiteMapField, rootAreaNodeType));
			type.Members.Add(source.NewField(RoutesField, routesNodeType));

			var actionWrapperName = @namespace + "." + node.PathNoSlashes + naming.ToActionWrapperName(node.Name);
			type.Members.Add(
				source.CreateReadOnlyProperty(
					"MyActions", 
					source[actionWrapperName],
				    new CodeObjectCreateExpression(
						source[actionWrapperName],
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(CodeGeneratorServicesField)))));

			var viewWrapperName = @namespace + "." + node.PathNoSlashes + naming.ToViewWrapperName(node.Name);
			type.Members.Add(
				source.CreateReadOnlyProperty(
					"MyViews", 
					source[viewWrapperName],
					new CodeObjectCreateExpression(
						source[viewWrapperName],
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(CodeGeneratorServicesField)))));

			var initialize = new CodeMemberMethod
			{
				Attributes = (MemberAttributes.Family),
				Name = "AddSiteMapToPropertyBag"
			};

			initialize.Statements.Add(AddPropertyToPropertyBag("MyViews"));
			initialize.Statements.Add(AddPropertyToPropertyBag("MyActions"));

			initialize.Statements.Add(new CodeAssignStatement(
				new CodeIndexerExpression(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "PropertyBag"),
					new CodePrimitiveExpression(RoutesProperty)),
				new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), RoutesProperty)));

			initialize.Statements.Add(new CodeAssignStatement(
				new CodeIndexerExpression(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "PropertyBag"),
					new CodePrimitiveExpression(SiteMapProperty)),
				new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), SiteMapProperty)));

			type.Members.Add(initialize);

			type.Members.Add(
				CreateMemberProperty
					.OfType(new CodeTypeReference(rootAreaNodeType))
					.Called(SiteMapProperty)
					.WithAttributes(MemberAttributes.Public)
					.WithCustomAttributes(source.DebuggerAttribute)
					.WithGetter(
						new CodeConditionStatement(
							new CodeBinaryOperatorExpression(
								new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(SiteMapField)),
								CodeBinaryOperatorType.ValueEquality,
								new CodePrimitiveExpression(null)),
							new CodeAssignStatement(
								new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(SiteMapField)),
								new CodeObjectCreateExpression(rootAreaNodeType, new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(CodeGeneratorServicesField))))),
						new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(SiteMapField))))
					.Property);

			type.Members.Add(
				CreateMemberProperty
					.OfType(new CodeTypeReference(routesNodeType))
					.Called(RoutesProperty)
					.WithAttributes(MemberAttributes.Public)
					.WithCustomAttributes(source.DebuggerAttribute)
					.WithGetter(
						new CodeConditionStatement(
							new CodeBinaryOperatorExpression(
								new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(RoutesField)),
								CodeBinaryOperatorType.ValueEquality,
								new CodePrimitiveExpression(null)),
							new CodeAssignStatement(
								new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(RoutesField)),
								new CodeObjectCreateExpression(routesNodeType, new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), EngineContextField)))),
						new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(RoutesField))))
					.Property);

			type.Members.Add(
				CreateMemberProperty
					.OfType<ICodeGeneratorServices>()
					.Called("CodeGeneratorServices")
					.WithAttributes(MemberAttributes.Public)
					.WithGetter(new CodeMethodReturnStatement(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(CodeGeneratorServicesField))))
					.WithSetter(new CodeAssignStatement(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), naming.ToMemberVariableName(CodeGeneratorServicesField)), 
						new CodePropertySetValueReferenceExpression()))
					.Property);

			base.Visit(node);
		}

		public override void Visit(WizardControllerTreeNode node)
		{
			Visit((ControllerTreeNode) node);
		}

		protected virtual CodeStatement AddPropertyToPropertyBag(string property)
		{
			return new CodeAssignStatement(
				new CodeIndexerExpression(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "PropertyBag"),
					new CodePrimitiveExpression(property)),
				new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), property));
		}
	}
}