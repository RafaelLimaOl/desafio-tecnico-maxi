import { Button } from "@/components/ui/button"
import {
  Empty,
  EmptyContent,
  EmptyDescription,
  EmptyHeader,
  EmptyMedia,
  EmptyTitle,
} from "@/components/ui/empty"

const Dashboard = () => {
  return (
    <div>
      <main className="flex">
        <Empty>
          <EmptyHeader>
            <EmptyMedia variant="icon"></EmptyMedia>
            <EmptyTitle>Dashboard</EmptyTitle>
            <EmptyDescription>Página sobre contrução</EmptyDescription>
          </EmptyHeader>
          <EmptyContent>
            <div className="flex gap-2">
              <Button>Ação</Button>
            </div>
          </EmptyContent>
        </Empty>
      </main>
    </div>
  )
}

export default Dashboard
