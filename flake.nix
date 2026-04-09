{
  description = "stakeholder-circus dotnet-stakeholder";

  inputs.nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";

  outputs = { self, nixpkgs }:
    let
      systems = [ "x86_64-linux" "aarch64-darwin" "x86_64-darwin" ];
      forAllSystems = nixpkgs.lib.genAttrs systems;
    in {
      devShells = forAllSystems (system:
        let pkgs = import nixpkgs { inherit system; };
        in {
          default = pkgs.mkShell {
            packages = with pkgs; [ dotnet-sdk_8 docker ];
          };
        });
      apps = forAllSystems (system:
        let pkgs = import nixpkgs { inherit system; };
            mk = name: text: {
              type = "app";
              program = "${pkgs.writeShellScript name text}";
            };
        in {
          build = mk "build" ''dotnet build dotnet-stakeholder.sln'';
          test = mk "test" ''dotnet test dotnet-stakeholder.sln'';
          check = mk "check" ''dotnet format dotnet-stakeholder.sln --verify-no-changes && dotnet build dotnet-stakeholder.sln && dotnet test dotnet-stakeholder.sln'';
          format = mk "format" ''dotnet format dotnet-stakeholder.sln --verify-no-changes'';
        });
    };
}
