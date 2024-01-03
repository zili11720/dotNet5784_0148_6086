
namespace DO;

public record Dependecy
(
  int Id,
  int? DependentTask=null,
  int? DependsOnTask=null
);
